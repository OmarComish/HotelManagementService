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
}