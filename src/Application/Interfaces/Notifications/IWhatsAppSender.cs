namespace HotelManagementService.Application.Interfaces.Notifications
{
    public interface IWhatsAppSender
    {
        bool isEnabled { get; }
        Task SendMessageAsync(
                string recipient,
                IReadOnlyList<string> parameters,
                CancellationToken cancellationToken = default);
    }
}