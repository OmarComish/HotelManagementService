using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class UserRepository: GenericRepository<User>, IUsersRepository
{
    public UserRepository(HotelDbContext context): base(context){}

    public async Task<User?> GetByUsernameWithRoleAsync(string username, CancellationToken ct = default)
    {
        var normalized = username.Trim().ToLowerInvariant();
        return await _dbSet
        .AsNoTracking()
        .Include(u => u.AssignedRole)
          .ThenInclude(r => r!.RolePermissions)
            .ThenInclude(rp =>rp.Permission)
        .FirstOrDefaultAsync(u => u.Username.ToLower()==normalized, ct);
    }
}