namespace MemoryPlaces.Application.Interfaces;

public interface IEmailSenderService
{
    Task SendEmailAsync(string recipientEmail, string subject, string messageBody);
}
