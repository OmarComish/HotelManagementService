using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Exceptions;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;

public class UserManagementService : IUserManagementService
{
    private const string ActivatePermission = "Users.Activate";
    private const string DeactivatePermission = "Users.Deactivate";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;

    public UserManagementService(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
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

        if ((role.IsSuperAdmin || userIsSuper) && !actor.isSuperAdmin)
            throw new RbacException(403, "Only an administrator can grant or revoke administrator access.");

        if (userIsSuper && !role.IsSuperAdmin &&
            await _unitOfWork.Users.CountActiveSuperAdminsAsync(ct) <= 1)
            throw new RbacException(409, "This is the last active administrator - the role cannot be removed.");

        user.RoleId = role.Id;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = actor.Username;
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

        user.Status = target;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = actor.Username;
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
}