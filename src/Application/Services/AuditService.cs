using System.Text.Json;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class AuditService : IAuditService
{
    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly IUnitOfWork _unitOfWork;
    private readonly IRequestContext _requestContext;

    public AuditService(IUnitOfWork unitOfWork, IRequestContext requestContext)
    {
        _unitOfWork = unitOfWork;
        _requestContext = requestContext;
    }

    public async Task RecordAsync(AuditEntry e)
    {
        var log = new AuditLog
        {
            ActorName = e.ActorName,
            Action = e.Action,
            EntityType = e.EntityType,
            EntityId = e.EntityId,
            EntityLabel = e.EntityLabel,
            IpAddress = Truncate(_requestContext.IpAddress, 64),
            UserAgent = Truncate(_requestContext.UserAgent, 512),
            PreviousValue = e.Previous is null ? null : JsonSerializer.Serialize(e.Previous, JsonOptions),
            NewValue = e.New is null ? null : JsonSerializer.Serialize(e.New, JsonOptions),
            CreatedBy = e.ActorUsername
        };

        await _unitOfWork.AuditLogs.AddAsync(log);
    }

    public async Task<PagedResult<AuditLogDto>> GetLogsAsync(AuditLogQuery query, CancellationToken ct = default)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var criteria = new AuditLogSearchCriteria
        {
            EntityType = IsAll(query.Entity) ? null : query.Entity!.Trim().ToLowerInvariant(),
            Action = IsAll(query.Action) ? null : query.Action!.Trim().ToLowerInvariant(),
            Search = query.Search,
            FromUtc = query.From is null
                ? null
                : DateTime.SpecifyKind(query.From.Value.Date, DateTimeKind.Utc),
            ToExclusiveUtc = query.To is null
                ? null
                : DateTime.SpecifyKind(query.To.Value.Date.AddDays(1), DateTimeKind.Utc),
            Skip = (page - 1) * pageSize,
            Take = pageSize
        };

        var (items, total) = await _unitOfWork.AuditLogs.SearchAsync(criteria, ct);
        return new PagedResult<AuditLogDto>(items.Select(ToDto).ToList(), total, page, pageSize);
    }

    private static bool IsAll(string? v) =>
        string.IsNullOrWhiteSpace(v) || v.Trim().Equals("all", StringComparison.OrdinalIgnoreCase);

    private static string? Truncate(string? v, int max) =>
        v is null ? null : (v.Length <= max ? v : v[..max]);

    private static JsonElement? ParseJson(string? json) =>
        string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<JsonElement>(json);

    private static AuditLogDto ToDto(AuditLog a) => new(
        a.Id,
        a.CreatedBy ?? string.Empty,
        a.ActorName,
        a.Action,
        a.EntityType,
        a.EntityId,
        a.EntityLabel,
        a.CreatedAt,
        a.IpAddress,
        a.UserAgent,
        ParseJson(a.PreviousValue),
        ParseJson(a.NewValue));
}