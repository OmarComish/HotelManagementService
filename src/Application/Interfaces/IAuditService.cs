using HotelManagementService.Application.DTOs;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.Interfaces;

public interface IRequestContext
{
    string? IpAddress { get; }
    string? UserAgent { get; }
}

public interface IAuditService
{
    /// Stages an audit row in the current unit of work. It is saved by the
    /// caller's next SaveChangesAsync, so it commits together with the change.
    Task RecordAsync(AuditEntry entry);

    Task<PagedResult<AuditLogDto>> GetLogsAsync(AuditLogQuery query, CancellationToken ct = default);
}