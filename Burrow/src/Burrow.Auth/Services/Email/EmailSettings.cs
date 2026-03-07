namespace Burrow.Auth.Services.Email;

public class EmailSettings
{
    public const string SectionName = "Email";
    public const string SmtpSender = "smtp";
    public const string FileSender = "file";

    public string Sender { get; set; } = "NoOp";
    public SmtpSettings Smtp { get; set; } = new();
    public FileEmailSettings File { get; set; } = new();
}

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
}

public class FileEmailSettings
{
    public string OutputDirectory { get; set; } = "/tmp/emails";
}
