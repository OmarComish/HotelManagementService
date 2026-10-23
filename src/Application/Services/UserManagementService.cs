using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Exceptions;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HotelManagementService.Application.Services;

public class UserManagementService : IUserManagementService
{
    private const string ActivatePermission = "Users.Activate";
    private const string DeactivatePermission = "Users.Deactivate";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    private readonly IAuditService _audit;
    IIdentityAdminClient _identity;

    public UserManagementService(IUnitOfWork unitOfWork, IPermissionService permissionService,
    IAuditService audit, IIdentityAdminClient identity)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
        _audit = audit;
        _identity = identity;
    }

    /* ---------- reads ---------- */

    public async Task<PagedResult<ManagedUserDto>> GetUsersAsync(UserQuery query, CancellationToken ct = default)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var criteria = new UserSearchCriteria
        {
            Search = query.Search,
            RoleKey = IsAll(query.Role) ? null : query.Role!.Trim().ToLowerInvariant(),
            Status = IsAll(query.Status) ? null : ParseStatus(query.Status),
            CreatedFromUtc = query.CreatedFrom is null
                ? null
                : DateTime.SpecifyKind(query.CreatedFrom.Value.Date, DateTimeKind.Utc),
            CreatedToExclusiveUtc = query.CreatedTo is null
                ? null
                : DateTime.SpecifyKind(query.CreatedTo.Value.Date.AddDays(1), DateTimeKind.Utc),
            SortBy = query.SortBy,
            SortDescending = string.Equals(query.SortDir, "desc", StringComparison.OrdinalIgnoreCase),
            Skip = (page - 1) * pageSize,
            Take = pageSize
        };

        var (items, total) = await _unitOfWork.Users.SearchAsync(criteria, ct);
        return new PagedResult<ManagedUserDto>(items.Select(ToDto).ToList(), total, page, pageSize);
    }

    public async Task<ManagedUserDto?> GetUserAsync(int id, CancellationToken ct = default)
    {
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(id, ct);
        return user is null ? null : ToDto(user);
    }

    public async Task<ManagedUserDto> UpdateUserAsync(
        string actorUsername, int id, UpdateUserRequest request, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var user = await _unitOfWork.Users.GetByIdForUpdateAsync(id, ct)
            ?? throw new RbacException(404, "User not found");

        var firstName = ValidateName(request.FirstName, "First name");
        var lastName = ValidateName(request.LastName, "Last name");
        var email = ValidateEmail(request.Email);

        if (await _unitOfWork.Users.EmailExistsAsync(email, user.Id, ct))
            throw new RbacException(409, "A user with that email already exists");

        var previous = new { firstName = user.FirstName, lastName = user.LastName, email = user.Email };

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = actor.Username;

        await _audit.RecordAsync(new AuditEntry(
            actor.Username, actor.FullName ?? actor.Username,
            "user.updated", "user", user.Id.ToString(), $"{firstName} {lastName}",
            previous,
            new { firstName, lastName, email }));

        await _unitOfWork.SaveChangesAsync();
        return (await GetUserAsync(id, ct))!;
    }

    private static string ValidateName(string? raw, string label)
    {
        var value = (raw ?? string.Empty).Trim();
        if (value.Length == 0 || value.Length > 50)
            throw new RbacException(422, $"{label} is required (max 50 characters)");
        return value;
    }

    private static string ValidateEmail(string? raw)
    {
        var value = (raw ?? string.Empty).Trim();
        if (value.Length == 0 || value.Length > 255 || !new EmailAddressAttribute().IsValid(value))
            throw new RbacException(422, "Enter a valid email address");
        return value;
    }

    /* ---------- mutations ---------- */

    public async Task<ManagedUserDto> ChangeRoleAsync(
        string actorUsername, int id, string roleKey, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var user = await _unitOfWork.Users.GetByIdForUpdateAsync(id, ct)
            ?? throw new RbacException(404, "User not found");

        if (IsSelf(actor, user))
            throw new RbacException(403, "You cannot change your own role.");

        var key = (roleKey ?? string.Empty).Trim().ToLowerInvariant();
        var role = await _unitOfWork.Roles.FirstOrDefaultAsync(r => r.Key == key);
        if (role is null || !role.IsActive)
            throw new RbacException(422, "Select a valid, active role");

        var userIsSuper = user.AssignedRole?.IsSuperAdmin == true;

        if ((role.IsSuperAdmin || userIsSuper) && !actor.IsSuperAdmin)
            throw new RbacException(403, "Only an administrator can grant or revoke administrator access.");

        if (userIsSuper && !role.IsSuperAdmin &&
            await _unitOfWork.Users.CountActiveSuperAdminsAsync(ct) <= 1)
            throw new RbacException(409, "This is the last active administrator - the role cannot be removed.");
        
        var previousRoleKey = user.AssignedRole?.Key;
        user.RoleId = role.Id;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = actor.Username;

        await _audit.RecordAsync(new AuditEntry(
            actor.Username, actor.FullName ?? actor.Username,
            "user.role_changed", "user", user.Id.ToString(), $"{user.FirstName} {user.LastName}",
            new { role = previousRoleKey },
            new { role = role.Key }));

        await _unitOfWork.SaveChangesAsync();

        return (await GetUserAsync(id, ct))!;
    }

    public async Task<ManagedUserDto> SetStatusAsync(
        string actorUsername, int id, string status, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);
        var target = ParseStatus(status);

        // Which permission applies depends on the requested status, so it's checked here.
        var needed = target == RecordStatus.Active ? ActivatePermission : DeactivatePermission;
        if (!actor.Permissions.Contains(needed))
            throw new RbacException(403,
                $"You do not have permission to perform this action ({needed}).");

        var user = await _unitOfWork.Users.GetByIdForUpdateAsync(id, ct)
            ?? throw new RbacException(404, "User not found");

        if (user.Status == target)
            return ToDto(user);

        if (target != RecordStatus.Active)
        {
            if (IsSelf(actor, user))
                throw new RbacException(403, "You cannot deactivate or lock your own account.");

            if (user.AssignedRole?.IsSuperAdmin == true &&
                await _unitOfWork.Users.CountActiveSuperAdminsAsync(ct) <= 1)
                throw new RbacException(409, "The last active administrator cannot be deactivated or locked.");
        }
        
        var previousStatus = user.Status;
        user.Status = target;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = actor.Username;

        await _audit.RecordAsync(new AuditEntry(
            actor.Username, actor.FullName ?? actor.Username,
            target == RecordStatus.Active ? "user.activated" : "user.deactivated",
            "user", user.Id.ToString(), $"{user.FirstName} {user.LastName}",
            new { status = StatusToString(previousStatus) },
            new { status = StatusToString(target) }));

        await _unitOfWork.SaveChangesAsync();

        return (await GetUserAsync(id, ct))!;
    }

    public async Task DeleteUserAsync(string actorUsername, int id, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var user = await _unitOfWork.Users.GetByIdForUpdateAsync(id, ct)
            ?? throw new RbacException(404, "User not found");

        if (IsSelf(actor, user))
            throw new RbacException(403, "You cannot delete your own account.");

        if (user.AssignedRole?.IsSuperAdmin == true &&
            await _unitOfWork.Users.CountActiveSuperAdminsAsync(ct) <= 1)
            throw new RbacException(409, "The last active administrator cannot be deleted.");

        var removed = await _identity.DeleteAccountAsync(user.Username, ct);
        if(!removed.Succeeded)
           throw new RbacException(502, $"The login account could not be removed: {removed.Error}");

        await _audit.RecordAsync(new AuditEntry(
            actor.Username, actor.FullName ?? actor.Username,
            "user.deleted", "user", user.Id.ToString(), $"{user.FirstName} {user.LastName}",
            new { username = user.Username, email = user.Email, role = user.AssignedRole?.Key },
            New: null));

        _unitOfWork.Users.Delete(user);
        await _unitOfWork.SaveChangesAsync();
    }

    /* ---------- helpers ---------- */

    private async Task<UserAccess> GetActorAsync(string username, CancellationToken ct) =>
        await _permissionService.GetAccessAsync(username, ct)
            ?? throw new RbacException(401, "Unauthorized");

    private static bool IsSelf(UserAccess actor, User user) =>
        string.Equals(actor.Username, user.Username, StringComparison.OrdinalIgnoreCase);

    private static bool IsAll(string? value) =>
        string.IsNullOrWhiteSpace(value) ||
        value.Trim().Equals("all", StringComparison.OrdinalIgnoreCase);

    private static RecordStatus ParseStatus(string? status) => status?.Trim().ToLowerInvariant() switch
    {
        "active" => RecordStatus.Active,
        "inactive" => RecordStatus.Disabled,
        _ => throw new RbacException(422, "Status must be 'active' or 'inactive'")
    };

    private static string StatusToString(RecordStatus s) => s switch
    {
        RecordStatus.Active => "active",
        RecordStatus.Disabled => "inactive",
        _ => s.ToString().ToLowerInvariant()
    };


    private static ManagedUserDto ToDto(User u) => new(
        u.Id,
        u.FirstName,
        u.LastName,
        $"{u.FirstName} {u.LastName}",
        u.Username,
        u.Email,
        u.AssignedRole?.Key,
        u.AssignedRole?.Name,
        StatusToString(u.Status),
        u.CreatedAt,
        u.UpdatedAt);


    public async Task<ManagedUserDto> CreateUserAsync(
        string actorUsername, CreateUserRequest request, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var firstName = ValidateName(request.FirstName, "First name");
        var lastName = ValidateName(request.LastName, "Last name");
        var username = ValidateUsername(request.Username);
        var email = ValidateEmail(request.Email);
        if (string.IsNullOrEmpty(request.Password))
            throw new RbacException(422, "Password is required");

        var roleKey = (request.RoleKey ?? string.Empty).Trim().ToLowerInvariant();
        var role = await _unitOfWork.Roles.FirstOrDefaultAsync(r => r.Key == roleKey);
        if (role is null || !role.IsActive)
            throw new RbacException(422, "Select a valid, active role");
        if (role.IsSuperAdmin && !actor.IsSuperAdmin)
            throw new RbacException(403, "You cannot assign the administrator role.");

        if (!await _unitOfWork.Users.DepartmentExistsAsync(request.DepartmentId, ct))
            throw new RbacException(422, "Select a valid department");
        if (await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username) is not null)
            throw new RbacException(409, "That username is already taken");
        if (await _unitOfWork.Users.EmailExistsAsync(email, 0, ct))
            throw new RbacException(409, "That email is already registered");

        User user;
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                DepartmentId = request.DepartmentId,
                CreatedBy = actor.Username
            };
            await _unitOfWork.Employees.AddAsync(employee);

            user = new User
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = "managed-by-identity-service",   // credentials live in IdentityService
                Status = RecordStatus.Active,
                Employee = employee,
                Role = role.IsSuperAdmin ? "Admin" : "Staff",    // legacy column, kept in sync
                RoleId = role.Id,
                CreatedBy = actor.Username
            };
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();                // ids are generated here

            await _audit.RecordAsync(new AuditEntry(
                actor.Username, actor.FullName ?? actor.Username,
                "user.created", "user", user.Id.ToString(), $"{firstName} {lastName}",
                Previous: null,
                New: new { username, email, role = role.Key, status = "active" }));
            await _unitOfWork.SaveChangesAsync();

            var account = await _identity.CreateAccountAsync(username, email, request.Password, ct);
            if (!account.Succeeded)
                throw new RbacException(422, account.Error ?? "The login account could not be created.");

            try
            {
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _identity.DeleteAccountAsync(username, ct);   // don't leave an orphan login
                throw;
            }
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        return (await GetUserAsync(user.Id, ct))!;
    }

    public async Task ResetPasswordAsync(
    string actorUsername, int id, string newPassword, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var user = await _unitOfWork.Users.GetByIdForUpdateAsync(id, ct)
            ?? throw new RbacException(404, "User not found");

        if (string.IsNullOrEmpty(newPassword))
            throw new RbacException(422, "Password is required");

        var result = await _identity.ResetPasswordAsync(user.Username, newPassword, ct);
        if (!result.Succeeded)
            throw new RbacException(422, $"Password requirements: {result.Error}");

        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = actor.Username;

        await _audit.RecordAsync(new AuditEntry(
            actor.Username, actor.FullName ?? actor.Username,
            "user.password_reset", "user", user.Id.ToString(), $"{user.FirstName} {user.LastName}",
            Previous: null,
            New: new { passwordReset = true }));
        await _unitOfWork.SaveChangesAsync();
    }

    private static string ValidateUsername(string? raw)
    {
        var value = (raw ?? string.Empty).Trim().ToLowerInvariant();
        if (!Regex.IsMatch(value, "^[a-z0-9._-]{3,30}$"))
            throw new RbacException(422, "Username must be 3-30 characters (letters, numbers, dot, dash, underscore)");
        return value;
    }
}