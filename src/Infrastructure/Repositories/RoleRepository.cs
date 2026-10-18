using HotelManagementService.Core.Entities;
using HotelManagementService.Infrastructure.Data;
using HotelManagementServices.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(HotelDbContext context) : base(context){}

    public Task<List<Role>> GetAllWithPermissionsAsync(CancellationToken ct = default) =>
        _context.Roles
            .AsNoTracking()
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .OrderBy(r => r.Name)
            .ToListAsync(ct);

    public Task<Role?> GetByIdWithPermissionsAsync(int id, CancellationToken ct = default) =>
        _context.Roles
            .AsNoTracking()
            .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<List<Permission>> GetAllPermissionsAsync(CancellationToken ct = default) =>
        _context.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Key)
            .ToListAsync(ct);
    public Task<Role?> GetByIdForUpdateAsync(int id, CancellationToken ct = default) =>
    _dbSet
        .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
        .FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<bool> KeyExistsAsync(string key, CancellationToken ct = default) =>
       _dbSet.AnyAsync(r => r.Key == key, ct);

    public Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken ct = default)
    {
        var lower = name.ToLowerInvariant();
        return _dbSet.AnyAsync(
            r => r.Name.ToLower() == lower && (excludeId == null || r.Id != excludeId), ct);
    }

    public Task<bool> HasUsersAsync(int roleId, CancellationToken ct = default) =>
        _context.Set<User>().AnyAsync(u => u.RoleId == roleId, ct);

    public void RemoveRolePermissions(IEnumerable<RolePermission> items) =>
        _context.Set<RolePermission>().RemoveRange(items);
}