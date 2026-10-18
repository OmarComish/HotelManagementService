using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class RoleService : IRoleService
{

    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
    private static RoleDto ToDto(Role r) => new(
        r.Id,
        r.Key,
        r.Name,
        r.Description,
        r.IsActive ? "active" : "inactive",
        r.IsSystem,
        r.IsSuperAdmin,
        r.RolePermissions.Select(rp => rp.Permission.Key).OrderBy(k => k).ToList());

 
}