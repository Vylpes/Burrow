using Microsoft.AspNetCore.Identity;
using Burrow.Auth.Data;

namespace Burrow.Auth.Services.Email;

public class FileEmailSender : IEmailSender<ApplicationUser>
{
    private readonly FileEmailSettings _settings;
    private readonly ILogger<FileEmailSender> _logger;

    public FileEmailSender(FileEmailSettings settings, ILogger<FileEmailSender> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href=\"{System.Net.WebUtility.HtmlDecode(confirmationLink).Replace("\"", "%22")}\">clicking here</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href=\"{System.Net.WebUtility.HtmlDecode(resetLink).Replace("\"", "%22")}\">clicking here</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");

    private async Task SendEmailAsync(string toAddress, string subject, string htmlMessage)
    {
        Directory.CreateDirectory(_settings.OutputDirectory);

        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        var invalidChars = Path.GetInvalidFileNameChars();
        var safeAddress = string.Concat(toAddress.Where(c => !invalidChars.Contains(c)));
        var fileName = $"{timestamp}_{safeAddress}_{Guid.NewGuid()}.html";
        var filePath = Path.Combine(_settings.OutputDirectory, fileName);

        var content = $"""
            To: {toAddress}
            Subject: {subject}
            Date: {DateTime.UtcNow:O}

            {htmlMessage}
            """;

        await File.WriteAllTextAsync(filePath, content);
        _logger.LogInformation("Email written to file: {FilePath}", filePath);
    }
}
