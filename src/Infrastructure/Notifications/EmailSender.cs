using HotelManagementService.Application.Interfaces.Notifications;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HotelManagementService.Infrastructure.Notifications;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(string recipient, string subject, string message,
        CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(
            _settings.SenderName,
            _settings.SenderEmail));

        email.To.Add(MailboxAddress.Parse(recipient));
        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = message
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(_settings.Host, _settings.Port,SecureSocketOptions.StartTls,cancellationToken);

        await smtp.AuthenticateAsync(_settings.Username,_settings.Password,cancellationToken);

        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}