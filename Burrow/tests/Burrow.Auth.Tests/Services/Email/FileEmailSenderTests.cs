using Burrow.Auth.Data;
using Burrow.Auth.Services.Email;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Burrow.Auth.Tests.Services.Email;

public class FileEmailSenderTests : IDisposable
{
    private readonly string _testOutputDirectory;
    private readonly FileEmailSettings _settings;
    private readonly ILogger<FileEmailSender> _logger;
    private readonly FileEmailSender _sender;

    public FileEmailSenderTests()
    {
        _testOutputDirectory = Path.Combine(Path.GetTempPath(), $"burrow_test_emails_{Guid.NewGuid()}");
        _settings = new FileEmailSettings { OutputDirectory = _testOutputDirectory };
        _logger = Substitute.For<ILogger<FileEmailSender>>();
        _sender = new FileEmailSender(_settings, _logger);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testOutputDirectory))
        {
            Directory.Delete(_testOutputDirectory, recursive: true);
        }
    }

    [Fact]
    public async Task SendConfirmationLinkAsync_CreatesFile()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendConfirmationLinkAsync(user, "test@example.com", "https://example.com/confirm?token=abc");

        var files = Directory.GetFiles(_testOutputDirectory);
        Assert.Single(files);
    }

    [Fact]
    public async Task SendConfirmationLinkAsync_FileContainsRecipientAddress()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendConfirmationLinkAsync(user, "test@example.com", "https://example.com/confirm?token=abc");

        var file = Directory.GetFiles(_testOutputDirectory).Single();
        var content = await File.ReadAllTextAsync(file);
        Assert.Contains("test@example.com", content);
    }

    [Fact]
    public async Task SendConfirmationLinkAsync_FileContainsConfirmEmailSubject()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendConfirmationLinkAsync(user, "test@example.com", "https://example.com/confirm?token=abc");

        var file = Directory.GetFiles(_testOutputDirectory).Single();
        var content = await File.ReadAllTextAsync(file);
        Assert.Contains("Confirm your email", content);
    }

    [Fact]
    public async Task SendPasswordResetLinkAsync_CreatesFile()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendPasswordResetLinkAsync(user, "test@example.com", "https://example.com/reset?token=abc");

        var files = Directory.GetFiles(_testOutputDirectory);
        Assert.Single(files);
    }

    [Fact]
    public async Task SendPasswordResetLinkAsync_FileContainsResetPasswordSubject()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendPasswordResetLinkAsync(user, "test@example.com", "https://example.com/reset?token=abc");

        var file = Directory.GetFiles(_testOutputDirectory).Single();
        var content = await File.ReadAllTextAsync(file);
        Assert.Contains("Reset your password", content);
    }

    [Fact]
    public async Task SendPasswordResetCodeAsync_CreatesFile()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendPasswordResetCodeAsync(user, "test@example.com", "123456");

        var files = Directory.GetFiles(_testOutputDirectory);
        Assert.Single(files);
    }

    [Fact]
    public async Task SendPasswordResetCodeAsync_FileContainsResetCode()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendPasswordResetCodeAsync(user, "test@example.com", "123456");

        var file = Directory.GetFiles(_testOutputDirectory).Single();
        var content = await File.ReadAllTextAsync(file);
        Assert.Contains("123456", content);
    }

    [Fact]
    public async Task SendEmailAsync_CreatesOutputDirectory_WhenNotExists()
    {
        Assert.False(Directory.Exists(_testOutputDirectory));
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendConfirmationLinkAsync(user, "test@example.com", "https://example.com/confirm");

        Assert.True(Directory.Exists(_testOutputDirectory));
    }

    [Fact]
    public async Task SendEmailAsync_FileNameContainsRecipientAddress()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendConfirmationLinkAsync(user, "test@example.com", "https://example.com/confirm");

        var file = Path.GetFileName(Directory.GetFiles(_testOutputDirectory).Single());
        // Invalid filename characters are stripped from the address; the domain portion always remains.
        Assert.Contains("example.com", file);
    }

    [Fact]
    public async Task SendEmailAsync_FileNameHasHtmlExtension()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        await _sender.SendConfirmationLinkAsync(user, "test@example.com", "https://example.com/confirm");

        var file = Path.GetFileName(Directory.GetFiles(_testOutputDirectory).Single());
        Assert.EndsWith(".html", file);
    }

    [Fact]
    public async Task SendEmailAsync_ConcurrentSends_CreateUniqueFiles()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => _sender.SendConfirmationLinkAsync(user, "test@example.com", "https://example.com/confirm"));

        await Task.WhenAll(tasks);

        var files = Directory.GetFiles(_testOutputDirectory);
        Assert.Equal(10, files.Length);
        Assert.Equal(files.Length, files.Distinct().Count());
    }
}
