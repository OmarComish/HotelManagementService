namespace HotelManagementService.Application.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task SendReservationConfirmationAsync(int reservationId,CancellationToken cancellationToken = default);
    }
}