using HotelManagementService.Core.Enums;

namespace HotelManagementService.Core.Entities;
public class Notification: BaseEntity
{
    public int? ReservationId { get; set; }

    public string Recipient { get; set; } = string.Empty;

    public string Channel { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;

    public string? ErrorMessage { get; set; }

    public DateTime? SentAt { get; set; }
}