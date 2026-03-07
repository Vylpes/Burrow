using Burrow.Auth.Components.Account;
using Burrow.Auth.Data;
using Burrow.Auth.Services.Email;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Burrow.Auth.Extensions;

public static class BurrowAuthExtensions
{
    /// <summary>
    /// Registers all Burrow.Auth identity and authentication services with the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration (used to read email settings).</param>
    public static IServiceCollection AddBurrowAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        services.AddAuthorization();

        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<BurrowAuthDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        var emailSettings = configuration.GetSection(EmailSettings.SectionName).Get<EmailSettings>() ?? new EmailSettings();

        switch (emailSettings.Sender.ToLowerInvariant())
        {
            case EmailSettings.SmtpSender:
                services.AddSingleton<IEmailSender<ApplicationUser>>(
                    new SmtpEmailSender(emailSettings.Smtp));
                break;
            case EmailSettings.FileSender:
                services.AddSingleton<IEmailSender<ApplicationUser>>(sp =>
                    new FileEmailSender(emailSettings.File, sp.GetRequiredService<ILogger<FileEmailSender>>()));
                break;
            default:
                services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
                break;
        }

        services.AddDbContext<BurrowAuthDbContext>(options =>
            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsAssembly(typeof(BurrowAuthDbContext).Assembly.GetName().Name!);
                o.MigrationsHistoryTable("__EFMigrationsHistory", "burrow");
            }));

        return services;
    }

    /// <summary>
    /// Maps the additional Burrow.Auth identity endpoints required by the Identity /Account Razor components.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static async Task<WebApplication> UseBurrowAuth(this WebApplication app)
    {
        app.MapAdditionalIdentityEndpoints();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<BurrowAuthDbContext>();
            await db.Database.MigrateAsync();
        }

        return app;
    }
}
