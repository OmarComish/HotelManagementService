namespace HotelManagementService.Core.Entities;

public class AuditLogQuery
{
    public string? Entity { get; set; }     // user | role | settings | auth; null or "all" = any
    public string? Action { get; set; }     // e.g. "user.role_changed"; null or "all" = any
    public string? Search { get; set; }     // actor or entity label
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}