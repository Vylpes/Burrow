using Burrow.Auth.Extensions;
using Burrow.Auth.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Burrow.Auth.Tests.Extensions;

public class BurrowAuthExtensionsTests
{
    private static IConfiguration CreateConfiguration(Dictionary<string, string?> settings)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    [Fact]
    public void AddBurrowAuth_ThrowsWhenConnectionStringMissing()
    {
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, string?>());

        var exception = Assert.Throws<InvalidOperationException>(() =>
            services.AddBurrowAuth(configuration));

        Assert.Contains("Connection string 'DefaultConnection' not found", exception.Message);
    }

    [Fact]
    public void AddBurrowAuth_ThrowsWhenSmtpHostMissing()
    {
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test",
            ["Email:Sender"] = "Smtp",
            ["Email:Smtp:Host"] = "",
            ["Email:Smtp:FromAddress"] = "test@example.com",
            ["Email:Smtp:FromName"] = "Test"
        });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            services.AddBurrowAuth(configuration));

        Assert.Contains("SMTP Host is required", exception.Message);
    }

    [Fact]
    public void AddBurrowAuth_ThrowsWhenSmtpFromAddressMissing()
    {
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test",
            ["Email:Sender"] = "Smtp",
            ["Email:Smtp:Host"] = "smtp.example.com",
            ["Email:Smtp:FromAddress"] = "",
            ["Email:Smtp:FromName"] = "Test"
        });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            services.AddBurrowAuth(configuration));

        Assert.Contains("SMTP FromAddress is required", exception.Message);
    }

    [Fact]
    public void AddBurrowAuth_ThrowsWhenSmtpFromNameMissing()
    {
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test",
            ["Email:Sender"] = "Smtp",
            ["Email:Smtp:Host"] = "smtp.example.com",
            ["Email:Smtp:FromAddress"] = "test@example.com",
            ["Email:Smtp:FromName"] = ""
        });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            services.AddBurrowAuth(configuration));

        Assert.Contains("SMTP FromName is required", exception.Message);
        Assert.Contains("appsettings.json", exception.Message);
    }

    [Fact]
    public void AddBurrowAuth_ConfiguresServicesWithNoOpEmailSender()
    {
        var services = new ServiceCollection();
        services.AddLogging(); // Required by some services
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test",
            ["Email:Sender"] = "NoOp"
        });

        services.AddBurrowAuth(configuration);

        var serviceProvider = services.BuildServiceProvider();

        // Verify key services are registered
        Assert.NotNull(serviceProvider.GetService<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider>());
        Assert.NotNull(serviceProvider.GetService<Burrow.Auth.Data.BurrowAuthDbContext>());
    }

    [Fact]
    public void AddBurrowAuth_ConfiguresServicesWithFileEmailSender()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test",
            ["Email:Sender"] = "File",
            ["Email:File:OutputDirectory"] = "/tmp/test-emails"
        });

        services.AddBurrowAuth(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Verify services are registered
        Assert.NotNull(serviceProvider.GetService<Burrow.Auth.Data.BurrowAuthDbContext>());
    }

    [Fact]
    public void AddBurrowAuth_IsCaseInsensitiveForEmailSender()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test",
            ["Email:Sender"] = "FILE" // Uppercase
        });

        // Should not throw
        services.AddBurrowAuth(configuration);
        var serviceProvider = services.BuildServiceProvider();

        Assert.NotNull(serviceProvider.GetService<Burrow.Auth.Data.BurrowAuthDbContext>());
    }

    [Fact]
    public void EmailSettings_DefaultsToNoOp()
    {
        var settings = new EmailSettings();

        Assert.Equal("NoOp", settings.Sender);
        Assert.NotNull(settings.Smtp);
        Assert.NotNull(settings.File);
    }

    [Fact]
    public void SmtpSettings_HasCorrectDefaults()
    {
        var settings = new SmtpSettings();

        Assert.Equal(587, settings.Port);
        Assert.True(settings.EnableSsl);
        Assert.Equal(string.Empty, settings.Host);
        Assert.Equal(string.Empty, settings.FromAddress);
        Assert.Equal(string.Empty, settings.FromName);
    }

    [Fact]
    public void FileEmailSettings_HasDefaultDirectory()
    {
        var settings = new FileEmailSettings();

        Assert.Equal("/tmp/emails", settings.OutputDirectory);
    }
}
