using MailKit.Net.Smtp;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MemoryPlaces.Infrastructure.Mail;

public class EmailSenderService : IEmailSenderService
{
    private readonly MailSettings _mailSettings;

    public EmailSenderService(IOptions<MailSettings> options)
    {
        _mailSettings = options.Value;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string messageBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(recipientEmail));
        message.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = messageBody };
        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(
                _mailSettings.Server,
                _mailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls
            );
            await smtp.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.Password);
            await smtp.SendAsync(message);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}
