using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IUsersRepository: IRepository<User>
{
    Task<User?> GetByUsernameWithRoleAsync(string username, CancellationToken ct = default);
    Task<(List<User> Items, int Total)> SearchAsync(UserSearchCriteria criteria, CancellationToken ct = default);
    Task<User?> GetByIdWithRoleAsync(int id, CancellationToken ct = default);
    Task<User?> GetByIdForUpdateAsync(int id, CancellationToken ct = default);
    Task<int> CountActiveSuperAdminsAsync(CancellationToken ct = default);
}