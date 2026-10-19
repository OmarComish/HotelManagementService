using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;

public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(HotelDbContext context) : base(context)
    {
    }

    public async Task<(List<AuditLog> Items, int Total)> SearchAsync(
        AuditLogSearchCriteria c, CancellationToken ct = default)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(c.EntityType))
            query = query.Where(a => a.EntityType == c.EntityType);
        if (!string.IsNullOrWhiteSpace(c.Action))
            query = query.Where(a => a.Action == c.Action);
        if (!string.IsNullOrWhiteSpace(c.Search))
        {
            var term = c.Search.Trim().ToLower();
            query = query.Where(a =>
                a.ActorName.ToLower().Contains(term) ||
                a.EntityLabel.ToLower().Contains(term) ||
                (a.CreatedBy != null && a.CreatedBy.ToLower().Contains(term)));
        }
        if (c.FromUtc.HasValue)
            query = query.Where(a => a.CreatedAt >= c.FromUtc.Value);
        if (c.ToExclusiveUtc.HasValue)
            query = query.Where(a => a.CreatedAt < c.ToExclusiveUtc.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.CreatedAt).ThenByDescending(a => a.Id)
            .Skip(c.Skip).Take(c.Take)
            .ToListAsync(ct);

        return (items, total);
    }
}