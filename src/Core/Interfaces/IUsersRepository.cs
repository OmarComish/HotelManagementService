using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IUsersRepository: IRepository<User>
{
    Task<User?> GetByUsernameWithRoleAsync(string username, CancellationToken ct = default);
}