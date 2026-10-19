using System.Text.RegularExpressions;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Exceptions;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using RoleEntity = HotelManagementService.Core.Entities.Role;

namespace HotelManagementService.Application.Services;

public class RoleService : IRoleService
{
    private const string AssignPermissionsKey = "Roles.AssignPermissions";
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    private readonly IAuditService _audit;
    public RoleService(IUnitOfWork unitOfWork, IPermissionService permissionService,
    IAuditService audit)
    {
        _unitOfWork = unitOfWork;
        _permissionService =permissionService;
        _audit = audit;
    }

    public async Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken ct = default)
    {
        var roles = await _unitOfWork.Roles.GetAllWithPermissionsAsync(ct);
        return roles.Select(ToDto).ToList();
    }

    public async Task<RoleDto?> GetRoleAsync(int id, CancellationToken ct = default)
    {
        var role = await _unitOfWork.Roles.GetByIdWithPermissionsAsync(id, ct);
        return role is null ? null : ToDto(role);
    }

    public async Task<IReadOnlyList<PermissionDto>> GetPermissionsAsync(CancellationToken ct = default)
    {
        var permissions = await _unitOfWork.Roles.GetAllPermissionsAsync(ct);
        return permissions
            .Select(p => new PermissionDto(p.Key, p.Name, p.Description, p.Key.Split('.')[0]))
            .ToList();
    }

     /* ---------- mutations ---------- */

    
    public async Task<RoleDto> CreateRoleAsync(string actorUsername, RoleInput input, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var name = ValidateName(input.Name);
        var isActive = ParseStatus(input.Status);
        var key = Slugify(name);
        if (key.Length == 0)
            throw new RbacException(422, "Role name must contain letters or numbers");

        if (await _unitOfWork.Roles.KeyExistsAsync(key, ct) ||
            await _unitOfWork.Roles.NameExistsAsync(name, null, ct))
            throw new RbacException(409, "A role with that name already exists");

        var permissions = await ResolvePermissionsAsync(input.Permissions, ct);
        if (permissions.Count > 0 && !actor.Permissions.Contains(AssignPermissionsKey))
            throw new RbacException(403, "You do not have permission to assign permissions (Roles.AssignPermissions).");

        var role = new RoleEntity
        {
            Key = key,
            Name = name,
            Description = CleanDescription(input.Description),
            IsActive = isActive,
            IsSystem = false,
            IsSuperAdmin = false,
            CreatedBy = actor.Username,
            RolePermissions = permissions
                .Select(p => new RolePermission { PermissionId = p.Id, CreatedBy = actor.Username })
                .ToList()
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();   // role.Id is generated here

            await _audit.RecordAsync(new AuditEntry(
                actor.Username, actor.FullName ?? actor.Username,
                "role.created", "role", role.Id.ToString(), role.Name,
                Previous: null,
                New: new
                {
                    key = role.Key,
                    status = isActive ? "active" : "inactive",
                    permissions = permissions.Select(p => p.Key).OrderBy(k => k).ToList()
                }));
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        return (await GetRoleAsync(role.Id, ct))!;
    }

    public async Task<RoleDto> UpdateRoleAsync(string actorUsername, int id, RoleInput input, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var role = await _unitOfWork.Roles.GetByIdForUpdateAsync(id, ct)
            ?? throw new RbacException(404, "Role not found");

        var permissions = await ResolvePermissionsAsync(input.Permissions, ct);
        var current = role.RolePermissions.Select(rp => rp.Permission.Key).ToHashSet();
        var requested = permissions.Select(p => p.Key).ToHashSet();
        var permsChanged = !current.SetEquals(requested);
        var isActive = ParseStatus(input.Status);

        if (permsChanged && !actor.Permissions.Contains(AssignPermissionsKey))
            throw new RbacException(403, "You do not have permission to modify role permissions (Roles.AssignPermissions).");
        if (permsChanged && role.Key == actor.RoleKey)
            throw new RbacException(403, "You cannot modify the permissions of your own role.");
        if (role.IsSuperAdmin && (permsChanged || !isActive))
            throw new RbacException(403, "The administrator role is protected and must keep full access.");

        var name = ValidateName(input.Name);
        if (await _unitOfWork.Roles.NameExistsAsync(name, role.Id, ct))
            throw new RbacException(409, "A role with that name already exists");
        if (role.IsActive && !isActive && await _unitOfWork.Roles.HasUsersAsync(role.Id, ct))
            throw new RbacException(409, "Reassign the users in this role before deactivating it.");

        var previous = new
        {
            name = role.Name,
            status = role.IsActive ? "active" : "inactive",
            permissions = current.OrderBy(k => k).ToList()
        };
        
        role.Name = name;
        role.Description = CleanDescription(input.Description);

        if (!role.IsSuperAdmin)
        {
            role.IsActive = isActive;

            if (permsChanged)
            {
                var toRemove = role.RolePermissions
                    .Where(rp => !requested.Contains(rp.Permission.Key))
                    .ToList();
                _unitOfWork.Roles.RemoveRolePermissions(toRemove);

                foreach (var p in permissions.Where(p => !current.Contains(p.Key)))
                    role.RolePermissions.Add(new RolePermission { PermissionId = p.Id, CreatedBy = actor.Username });
            }
        }

        role.UpdatedAt = DateTime.UtcNow;
        role.UpdatedBy = actor.Username;

        await _audit.RecordAsync(new AuditEntry(
        actor.Username, actor.FullName ?? actor.Username,
        permsChanged ? "role.permissions_changed" : "role.updated",
        "role", role.Id.ToString(), role.Name,
        previous,
        new
        {
            name = role.Name,
            status = role.IsActive ? "active" : "inactive",
            permissions = (permsChanged ? requested : current).OrderBy(k => k).ToList()
        }));
        await _unitOfWork.SaveChangesAsync();

        return (await GetRoleAsync(role.Id, ct))!;
    }

    public async Task DeleteRoleAsync(string actorUsername, int id, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var role = await _unitOfWork.Roles.GetByIdForUpdateAsync(id, ct)
            ?? throw new RbacException(404, "Role not found");

        if (role.IsSystem)
            throw new RbacException(403, "System roles cannot be deleted.");
        if (await _unitOfWork.Roles.HasUsersAsync(role.Id, ct))
            throw new RbacException(409, "Reassign the users in this role before deleting it.");

        var previous = new
        {
            key = role.Key,
            permissions = role.RolePermissions.Select(rp => rp.Permission.Key).OrderBy(k => k).ToList()
        };

        _unitOfWork.Roles.RemoveRolePermissions(role.RolePermissions.ToList());
        _unitOfWork.Roles.Delete(role);

        await _audit.RecordAsync(new AuditEntry(
            actor.Username, actor.FullName ?? actor.Username,
            "role.deleted", "role", role.Id.ToString(), role.Name,
            previous, New: null));

        await _unitOfWork.SaveChangesAsync();
    }

    /* ---------- helpers ---------- */

    private async Task<UserAccess> GetActorAsync(string username, CancellationToken ct) =>
        await _permissionService.GetAccessAsync(username, ct)
            ?? throw new RbacException(401, "Unauthorized");

    private static string ValidateName(string? raw)
    {
        var name = (raw ?? string.Empty).Trim();
        if (name.Length == 0 || name.Length > 60)
            throw new RbacException(422, "Role name is required (max 60 characters)");
        return name;
    }

    private static bool ParseStatus(string? status) => status?.ToLowerInvariant() switch
    {
        "active" => true,
        "inactive" => false,
        _ => throw new RbacException(422, "Status must be 'active' or 'inactive'")
    };

    private static string Slugify(string name) =>
        Regex.Replace(name.ToLowerInvariant(), "[^a-z0-9]+", "_").Trim('_');

    private static string CleanDescription(string? description)
    {
        var d = (description ?? string.Empty).Trim();
        return d.Length > 240 ? d[..240] : d;
    }

    // Unknown permission keys are ignored, as ALL_PERMISSIONS.includes() did in rbac.ts.
    private async Task<List<Permission>> ResolvePermissionsAsync(
        IReadOnlyList<string>? keys, CancellationToken ct)
    {
        var wanted = new HashSet<string>(keys ?? Array.Empty<string>(), StringComparer.Ordinal);
        var catalog = await _unitOfWork.Roles.GetAllPermissionsAsync(ct);
        return catalog.Where(p => wanted.Contains(p.Key)).ToList();
    }
    private static RoleDto ToDto(Role r) => new(
        r.Id,
        r.Key,
        r.Name,
        r.Description,
        r.IsActive ? "active" : "inactive",
        r.IsSystem,
        r.IsSuperAdmin,
        r.RolePermissions.Select(rp => rp.Permission.Key).OrderBy(k => k).ToList(),
        r.CreatedAt,
        r.UpdatedAt);

 
}