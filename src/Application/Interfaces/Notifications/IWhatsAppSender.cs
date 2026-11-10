namespace HotelManagementService.Application.Interfaces.Notifications
{
    public interface IWhatsAppSender
    {
        Task SendMessageAsync(
            string recipient,
            string message,
            CancellationToken cancellationToken = default);
    }
}