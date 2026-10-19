namespace HotelManagementService.Core.Entities;
public class AuditLogSearchCriteria
{
    public string? EntityType { get; set; }
    public string? Action { get; set; }
    public string? Search { get; set; }
    public DateTime? FromUtc { get; set; }
    public DateTime? ToExclusiveUtc { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 20;
}