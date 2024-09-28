namespace MemoryPlaces.Infrastructure.Settings;

public class MailSettings
{
    public string Server { get; set; } = default!;
    public int Port { get; set; }
    public string SenderEmail { get; set; } = default!;
    public string SenderName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool EnableSsl { get; set; }
}
