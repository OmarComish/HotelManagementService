using HotelManagementService.Core.Entities;
//using HotelManagementService.Core.Models;

namespace HotelManagementService.Core.Interfaces;

public interface IAuditLogRepository : IRepository<AuditLog>
{
    Task<(List<AuditLog> Items, int Total)> SearchAsync(AuditLogSearchCriteria criteria, CancellationToken ct = default);
}