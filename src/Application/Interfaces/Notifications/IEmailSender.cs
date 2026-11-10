namespace HotelManagementService.Application.Interfaces.Notifications
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            string recipient,
            string subject,
            string message,
            CancellationToken cancellationToken = default);
    }
}