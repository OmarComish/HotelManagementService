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
    public async Task<(List<User> Items, int Total)> SearchAsync(UserSearchCriteria c, CancellationToken ct = default)
    {
        var query = _dbSet.AsNoTracking().Include(u => u.AssignedRole).AsQueryable();

        if (!string.IsNullOrWhiteSpace(c.Search))
        {
            var term = c.Search.Trim().ToLower();
            query = query.Where(u =>
                (u.FirstName + " " + u.LastName).ToLower().Contains(term) ||
                u.Username.ToLower().Contains(term) ||
                u.Email.ToLower().Contains(term));
        }
        if (!string.IsNullOrWhiteSpace(c.RoleKey))
            query = query.Where(u => u.AssignedRole != null && u.AssignedRole.Key == c.RoleKey);
        if (c.Status.HasValue)
            query = query.Where(u => u.Status == c.Status.Value);
        if (c.CreatedFromUtc.HasValue)
            query = query.Where(u => u.CreatedAt >= c.CreatedFromUtc.Value);
        if (c.CreatedToExclusiveUtc.HasValue)
            query = query.Where(u => u.CreatedAt < c.CreatedToExclusiveUtc.Value);

        var desc = c.SortDescending;
        IOrderedQueryable<User> ordered = (c.SortBy?.ToLowerInvariant()) switch
        {
            "username"  => desc ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
            "email"     => desc ? query.OrderByDescending(u => u.Email)    : query.OrderBy(u => u.Email),
            "status"    => desc ? query.OrderByDescending(u => u.Status)   : query.OrderBy(u => u.Status),
            "createdat" => desc ? query.OrderByDescending(u => u.CreatedAt): query.OrderBy(u => u.CreatedAt),
            _           => desc ? query.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName)
                                : query.OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
        };

        var total = await ordered.CountAsync(ct);
        var items = await ordered.ThenBy(u => u.Id).Skip(c.Skip).Take(c.Take).ToListAsync(ct);
        return (items, total);
    }

    public Task<User?> GetByIdWithRoleAsync(int id, CancellationToken ct = default) =>
        _dbSet.AsNoTracking().Include(u => u.AssignedRole).FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<User?> GetByIdForUpdateAsync(int id, CancellationToken ct = default) =>
        _dbSet.Include(u => u.AssignedRole).FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<int> CountActiveSuperAdminsAsync(CancellationToken ct = default) =>
        _dbSet.CountAsync(u => u.Status == RecordStatus.Active
                            && u.AssignedRole != null
                            && u.AssignedRole.IsSuperAdmin, ct);
}