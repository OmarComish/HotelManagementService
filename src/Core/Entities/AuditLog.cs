namespace HotelManagementService.Core.Entities;
public class AuditLog: BaseEntity
{
    public string ActorName { get; set; } = null!;
    public string Action { get; set; } = null!;        // e.g. "user.role_changed"
    public string EntityType { get; set; } = null!;    // "user" | "role" | "settings" | "auth"
    public string? EntityId { get; set; }
    public string EntityLabel { get; set; } = null!;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? PreviousValue { get; set; }         // JSON
    public string? NewValue { get; set; }              // JSON
}