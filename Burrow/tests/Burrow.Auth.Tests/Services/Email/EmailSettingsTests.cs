using Burrow.Auth.Services.Email;

namespace Burrow.Auth.Tests.Services.Email;

public class EmailSettingsTests
{
    [Fact]
    public void EmailSettings_DefaultSender_IsNoOp()
    {
        var settings = new EmailSettings();
        Assert.Equal("NoOp", settings.Sender);
    }

    [Fact]
    public void EmailSettings_DefaultSmtp_IsEmpty()
    {
        var settings = new EmailSettings();
        Assert.NotNull(settings.Smtp);
        Assert.Equal(string.Empty, settings.Smtp.Host);
        Assert.Equal(587, settings.Smtp.Port);
        Assert.Equal(string.Empty, settings.Smtp.Username);
        Assert.Equal(string.Empty, settings.Smtp.Password);
        Assert.True(settings.Smtp.EnableSsl);
        Assert.Equal(string.Empty, settings.Smtp.FromAddress);
        Assert.Equal("Droplet", settings.Smtp.FromName);
    }

    [Fact]
    public void EmailSettings_DefaultFile_IsDefaultDirectory()
    {
        var settings = new EmailSettings();
        Assert.NotNull(settings.File);
        Assert.Equal("/tmp/emails", settings.File.OutputDirectory);
    }

    [Fact]
    public void EmailSettings_SectionName_IsEmail()
    {
        Assert.Equal("Email", EmailSettings.SectionName);
    }

    [Fact]
    public void EmailSettings_SmtpSender_Constant()
    {
        Assert.Equal("smtp", EmailSettings.SmtpSender);
    }

    [Fact]
    public void EmailSettings_FileSender_Constant()
    {
        Assert.Equal("file", EmailSettings.FileSender);
    }
}
