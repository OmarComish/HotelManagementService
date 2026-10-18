using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementServices.Core.Interfaces;
public interface IRoleRepository: IRepository<Role>
{
    Task<List<Role>> GetAllWithPermissionsAsync(CancellationToken ct = default);
    Task<Role?> GetByIdWithPermissionsAsync(int id, CancellationToken ct = default);
    Task<List<Permission>> GetAllPermissionsAsync(CancellationToken ct = default);
}