using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IRoleService
{
    Task<IReadOnlyList<RoleDto>> GetRolesAsync(CancellationToken ct = default);
    Task<RoleDto?> GetRoleAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PermissionDto>> GetPermissionsAsync(CancellationToken ct = default);
}