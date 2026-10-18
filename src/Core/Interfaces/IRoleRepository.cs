using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementServices.Core.Interfaces;
public interface IRoleRepository: IRepository<Role>
{
    Task<List<Role>> GetAllWithPermissionsAsync(CancellationToken ct = default);
    Task<Role?> GetByIdWithPermissionsAsync(int id, CancellationToken ct = default);
    Task<List<Permission>> GetAllPermissionsAsync(CancellationToken ct = default);
    Task<Role?> GetByIdForUpdateAsync(int id, CancellationToken ct = default);   // tracked
    Task<bool> KeyExistsAsync(string key, CancellationToken ct = default);
    Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken ct = default);
    Task<bool> HasUsersAsync(int roleId, CancellationToken ct = default);
    void RemoveRolePermissions(IEnumerable<RolePermission> items);
}