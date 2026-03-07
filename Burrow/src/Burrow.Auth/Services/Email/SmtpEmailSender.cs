using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Burrow.Auth.Data;

namespace Burrow.Auth.Services.Email;

public class SmtpEmailSender : IEmailSender<ApplicationUser>
{
    private readonly SmtpSettings _settings;

    public SmtpEmailSender(SmtpSettings settings)
    {
        _settings = settings;
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href=\"{System.Net.WebUtility.HtmlDecode(confirmationLink).Replace("\"", "%22")}\">clicking here</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href=\"{System.Net.WebUtility.HtmlDecode(resetLink).Replace("\"", "%22")}\">clicking here</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");

    internal MailMessage BuildMailMessage(string toAddress, string subject, string htmlMessage)
    {
        var from = new MailAddress(_settings.FromAddress, _settings.FromName);
        var to = new MailAddress(toAddress);

        return new MailMessage(from, to)
        {
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };
    }

    private async Task SendEmailAsync(string toAddress, string subject, string htmlMessage)
    {
        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
        };

        using var message = BuildMailMessage(toAddress, subject, htmlMessage);

        await client.SendMailAsync(message);
    }
}
