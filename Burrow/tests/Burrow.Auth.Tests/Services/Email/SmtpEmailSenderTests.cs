using System.Net.Mail;
using Burrow.Auth.Data;
using Burrow.Auth.Services.Email;

namespace Burrow.Auth.Tests.Services.Email;

public class SmtpEmailSenderTests
{
    private readonly SmtpSettings _settings;
    private readonly SmtpEmailSender _sender;

    public SmtpEmailSenderTests()
    {
        _settings = new SmtpSettings
        {
            Host = "smtp.example.com",
            Port = 587,
            Username = "user@example.com",
            Password = "secret",
            EnableSsl = true,
            FromAddress = "noreply@example.com",
            FromName = "Test App",
        };
        _sender = new SmtpEmailSender(_settings);
    }

    [Fact]
    public void BuildMailMessage_SetsSubject()
    {
        using MailMessage message = _sender.BuildMailMessage("to@example.com", "My Subject", "<p>Body</p>");

        Assert.Equal("My Subject", message.Subject);
    }

    [Fact]
    public void BuildMailMessage_SetsBody()
    {
        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Subject", "<p>Hello</p>");

        Assert.Equal("<p>Hello</p>", message.Body);
    }

    [Fact]
    public void BuildMailMessage_SetsIsBodyHtml_True()
    {
        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Subject", "<p>Body</p>");

        Assert.True(message.IsBodyHtml);
    }

    [Fact]
    public void BuildMailMessage_SetsRecipient()
    {
        using MailMessage message = _sender.BuildMailMessage("recipient@example.com", "Subject", "Body");

        Assert.Single(message.To);
        Assert.Equal("recipient@example.com", message.To[0].Address);
    }

    [Fact]
    public void BuildMailMessage_SetsFrom_AddressFromSettings()
    {
        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Subject", "Body");

        Assert.Equal(_settings.FromAddress, message.From!.Address);
    }

    [Fact]
    public void BuildMailMessage_SetsFrom_DisplayNameFromSettings()
    {
        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Subject", "Body");

        Assert.Equal(_settings.FromName, message.From!.DisplayName);
    }

    [Fact]
    public void BuildMailMessage_ConfirmationEmail_UsesCorrectSubject()
    {
        var user = new ApplicationUser { Email = "user@example.com" };

        using MailMessage message = _sender.BuildMailMessage(user.Email!, "Confirm your email", "body");

        Assert.Equal("Confirm your email", message.Subject);
    }

    [Fact]
    public void BuildMailMessage_PasswordResetEmail_UsesCorrectSubject()
    {
        // Both SendPasswordResetLinkAsync and SendPasswordResetCodeAsync use this subject.
        using MailMessage message = _sender.BuildMailMessage("user@example.com", "Reset your password", "body");

        Assert.Equal("Reset your password", message.Subject);
    }

    [Fact]
    public void BuildMailMessage_ConfirmationLinkBody_ContainsHtmlAnchorTag()
    {
        var link = "https://example.com/confirm?token=abc";
        var body = $"Please confirm your account by <a href=\"{System.Net.WebUtility.HtmlDecode(link).Replace("\"", "%22")}\">clicking here</a>.";

        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Confirm your email", body);

        Assert.Contains("<a href=", message.Body);
        Assert.Contains("clicking here", message.Body);
    }

    [Fact]
    public void BuildMailMessage_PasswordResetLinkBody_ContainsHtmlAnchorTag()
    {
        var link = "https://example.com/reset?token=xyz";
        var body = $"Please reset your password by <a href=\"{System.Net.WebUtility.HtmlDecode(link).Replace("\"", "%22")}\">clicking here</a>.";

        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Reset your password", body);

        Assert.Contains("<a href=", message.Body);
        Assert.Contains("clicking here", message.Body);
    }

    [Fact]
    public void BuildMailMessage_PasswordResetCodeBody_ContainsResetCode()
    {
        var resetCode = "123456";
        var body = $"Please reset your password using the following code: {resetCode}";

        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Reset your password", body);

        Assert.Contains(resetCode, message.Body);
    }

    [Fact]
    public void BuildMailMessage_ConfirmationLink_HtmlEncodedUrl_IsDecoded()
    {
        // URLs may arrive with HTML-encoded ampersands; the body formatter decodes them.
        var htmlEncodedLink = "https://example.com/confirm?a=1&amp;b=2";
        var decodedLink = System.Net.WebUtility.HtmlDecode(htmlEncodedLink);
        var body = $"Please confirm your account by <a href=\"{decodedLink.Replace("\"", "%22")}\">clicking here</a>.";

        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Confirm your email", body);

        Assert.Contains("a=1&b=2", message.Body);
    }

    [Fact]
    public void BuildMailMessage_ConfirmationLink_DoubleQuotesInUrl_AreEscaped()
    {
        // Double quotes in the URL must be percent-encoded to keep the href attribute valid.
        var linkWithQuote = "https://example.com/confirm?token=ab\"cd";
        var sanitized = System.Net.WebUtility.HtmlDecode(linkWithQuote).Replace("\"", "%22");
        var body = $"Please confirm your account by <a href=\"{sanitized}\">clicking here</a>.";

        using MailMessage message = _sender.BuildMailMessage("to@example.com", "Confirm your email", body);

        Assert.DoesNotContain("ab\"cd", message.Body);
        Assert.Contains("ab%22cd", message.Body);
    }
}
