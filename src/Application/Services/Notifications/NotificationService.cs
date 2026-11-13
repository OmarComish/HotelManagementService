using HotelManagementService.Application.Interfaces.Notifications;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Enums;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IWhatsAppSender _whatsAppSender;

    public NotificationService(
        IUnitOfWork unitOfWork,
        IEmailSender emailSender,
        IWhatsAppSender whatsAppSender)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _whatsAppSender = whatsAppSender;
    }

    public async Task SendReservationConfirmationAsync( int reservationId,
    CancellationToken cancellationToken = default)
    {
        var reservation = await _unitOfWork.Reservations
            .GetWithGuestAndRoomByIdAsync(reservationId);

        if (reservation == null)
            throw new InvalidOperationException(
                $"Reservation {reservationId} was not found.");

        var guestName = $"{reservation.Guest.FirstName} {reservation.Guest.LastName}";

        var message = $"""
            Dear {guestName},

            Your reservation has been received successfully.

            Room: {reservation.Room.RoomNumber}
            Check-in: {reservation.CheckIn:dd MMM yyyy}
            Check-out: {reservation.CheckOut:dd MMM yyyy}
            Guests: {reservation.Guests}
            Total Amount: {reservation.TotalAmount:N2}

            We look forward to welcoming you.

            Hotel Management Team
            """;

        await SendAndRecordAsync(
            reservationId,
            reservation.Email,
            "Email",
            "Reservation Confirmation",
            message,
            (token) => _emailSender.SendEmailAsync(
                reservation.Email, "Reservation Confirmation", message, token),
            cancellationToken);
            
        if( _whatsAppSender.isEnabled)
        {
             await SendAndRecordAsync(
            reservationId,
            reservation.Phone,
            "WhatsApp",
            string.Empty,
            message,
            (token) => _whatsAppSender.SendMessageAsync(
                reservation.Phone, new[]
                {
                    guestName,
                    reservation.Room.RoomNumber,
                    reservation.CheckIn.ToString("dd MMM yyyy"),
                    reservation.CheckOut.ToString("dd MMM yyyy"),
                    reservation.Guests.ToString(),
                    reservation.TotalAmount.ToString("N2")
                }, token),
            cancellationToken);
        }
       
    }

    private async Task SendAndRecordAsync(int reservationId,string recipient,string channel,string subject,
        string message,
        Func<CancellationToken, Task> send,
        CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            ReservationId = reservationId,
            Recipient = recipient,
            Channel = channel,
            Subject = subject,
            Message = message,
            Status = NotificationStatus.Pending
        };

        await _unitOfWork.Notifications.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        try
        {
            await send(cancellationToken);

            notification.Status = NotificationStatus.Sent;
            notification.SentAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            notification.Status = NotificationStatus.Failed;
            notification.ErrorMessage = ex.Message;
        }

        await _unitOfWork.SaveChangesAsync();
    }
}