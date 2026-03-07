# Module Management

Burrow is designed with modularity at its core. This guide explains how to add, remove, and manage Burrow modules in your project.

## Available Modules

| Module | Description | Status | Dependencies |
|--------|-------------|--------|--------------|
| **Burrow.Auth** | Authentication with ASP.NET Core Identity | ✅ Stable | PostgreSQL, EF Core |
| Burrow.Core | Shared utilities (only if needed) | 💭 Planned | None |
| Burrow.Payment | Payment processing | 💭 Future | To be determined |
| Burrow.Messaging | Email templates & notifications | 💭 Future | To be determined |

## Understanding Modules

Each Burrow module is:
- **Self-contained**: Lives in `Burrow/src/Burrow.{ModuleName}/`
- **Optional**: Include only what you need
- **Tested**: Has corresponding test project in `Burrow/tests/`
- **Documented**: Architecture explained in `docs/architecture/`

## Adding Burrow.Auth to Your Project

If you started with the Example.Web template, Burrow.Auth is already integrated. If you're adding it to a new project:

### Step 1: Add Project Reference

Edit your `.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="../../Burrow/src/Burrow.Auth/Burrow.Auth.csproj" />
</ItemGroup>
```

### Step 2: Configure Services

In `Program.cs`:

```csharp
using Burrow.Auth.Extensions;
using Burrow.Auth.Components.Account;

var builder = WebApplication.CreateBuilder(args);

// Add Burrow.Auth services
builder.Services.AddBurrowAuth(builder.Configuration);

// ... other services ...

var app = builder.Build();

// Use Burrow.Auth middleware and run migrations
await app.UseBurrowAuth();

// Add Burrow.Auth Razor components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(IdentityRedirectManager).Assembly);

await app.RunAsync();
```

### Step 3: Configure Authentication Routes

In your `Routes.razor`:

```razor
@using Burrow.Auth.Components.Account.Shared

<Router AppAssembly="typeof(Program).Assembly">
    <Found Context="routeData">
        <AuthorizeRouteView RouteData="routeData" DefaultLayout="typeof(MainLayout)">
            <NotAuthorized>
                <RedirectToLogin />
            </NotAuthorized>
        </AuthorizeRouteView>
    </Found>
</Router>
```

### Step 4: Add Styles

In your `App.razor` (inside `<head>`):

```html
<link rel="stylesheet" href="_content/Burrow.Auth/app.css" />
```

### Step 5: Configure Email and Database

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=yourdb;Username=user;Password=pass"
  },
  "Email": {
    "Sender": "File",
    "File": {
      "OutputDirectory": ".temp/emails"
    }
  }
}
```

See [Getting Started - Email Configuration](getting-started.md#email-configuration) for SMTP setup.

### Step 6: Run Migrations

Burrow.Auth auto-runs migrations on startup (in development), or manually:

```bash
cd Burrow/src/Burrow.Auth
dotnet ef database update
```

### Step 7: Test

Run the app and navigate to `/Account/Register` to create an account.

## Removing Burrow.Auth

If you decide you don't need authentication:

### Step 1: Remove Project Reference

Edit your `.csproj` and remove:

```xml
<ProjectReference Include="../../Burrow/src/Burrow.Auth/Burrow.Auth.csproj" />
```

### Step 2: Remove Service Configuration

In `Program.cs`, remove:

```csharp
builder.Services.AddBurrowAuth(builder.Configuration);
// ...
await app.UseBurrowAuth();
// ...
.AddAdditionalAssemblies(typeof(IdentityRedirectManager).Assembly);
```

### Step 3: Remove Routes

In `Routes.razor`, remove:

```razor
@using Burrow.Auth.Components.Account.Shared
// and
<RedirectToLogin />
```

### Step 4: Remove Styles

In `App.razor`, remove:

```html
<link rel="stylesheet" href="_content/Burrow.Auth/app.css" />
```

### Step 5: Clean Database

If you want to remove authentication tables:

```sql
DROP SCHEMA burrow CASCADE;
```

Or just ignore them - they won't affect your app since they're in a separate schema.

### Step 6: Clean Build

```bash
dotnet clean
dotnet build
```

## Module-Specific Configuration

### Burrow.Auth Configuration Options

#### Required

- `ConnectionStrings:DefaultConnection` - Database connection
- `Email:Smtp:FromName` - Email sender name (if using SMTP)
- `Email:Smtp:FromAddress` - Email sender address (if using SMTP)
- `Email:Smtp:Host` - SMTP server (if using SMTP)

#### Optional

- `Email:Sender` - Email strategy: "File", "Smtp", or "NoOp" (default: "NoOp")
- `Email:Smtp:Port` - SMTP port (default: 587)
- `Email:Smtp:Username` - SMTP auth username
- `Email:Smtp:Password` - SMTP auth password
- `Email:Smtp:EnableSsl` - Use SSL/TLS (default: true)
- `Email:File:OutputDirectory` - Where to save development emails (default: "/tmp/emails")

#### Example Full Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=myapp;Username=dbuser;Password=dbpass"
  },
  "Email": {
    "Sender": "Smtp",
    "Smtp": {
      "Host": "smtp.sendgrid.net",
      "Port": 587,
      "Username": "apikey",
      "Password": "SG.your-sendgrid-api-key",
      "EnableSsl": true,
      "FromAddress": "noreply@myapp.com",
      "FromName": "My Application"
    }
  }
}
```

## Creating Custom Modules (Advanced)

If you want to create your own Burrow-style module:

### Module Structure

```
Burrow/src/Burrow.YourModule/
├── Burrow.YourModule.csproj
├── Extensions/
│   └── Burrow YourModuleExtensions.cs   # Public API
├── Services/
│   └── YourService.cs                   # Internal logic
├── Data/
│   └── YourModuleDbContext.cs           # If needed
└── Components/                          # If Blazor components
```

### Extension Method Pattern

Follow the pattern from Burrow.Auth:

```csharp
public static class BurrowYourModuleExtensions
{
    public static IServiceCollection AddBurrowYourModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register services
        // Validate configuration
        // Add DbContext if needed
        return services;
    }

    public static WebApplication UseBurrowYourModule(
        this WebApplication app)
    {
        // Add middleware
        // Map endpoints
        // Run migrations
        return app;
    }
}
```

### Module Guidelines

1. **Namespaces**: Use `Burrow.{ModuleName}` as root namespace
2. **Configuration**: Validate early, fail fast with helpful error messages
3. **Database**: Use dedicated schema (e.g., `burrow_yourmodule`)
4. **Testing**: Create `Burrow.{ModuleName}.Tests` project
5. **Documentation**: Add architecture doc in `docs/architecture/`

## Module Dependencies

Currently, all modules are independent. If a future module requires another module (e.g., Burrow.Payment might require Burrow.Auth):

1. Document the dependency clearly in module README
2. Check for the dependency in extension method and throw helpful error if missing
3. Consider making it optional with graceful degradation

## Module Versions

Right now, all modules share the same version as Burrow itself (see `Burrow/version.txt`).

In the future, modules might have independent versioning if they evolve at different rates.

## Troubleshooting

### "Connection string 'DefaultConnection' not found"

**Cause**: Missing database configuration

**Solution**: Add connection string to `appsettings.json` or environment variables

### "SMTP FromName is required"

**Cause**: Using SMTP email sender without required configuration

**Solution**: Either:
1. Configure all SMTP settings in `appsettings.json`
2. Switch to `"Sender": "File"` for development
3. Use `"Sender": "NoOp"` if you don't need emails

### Migrations Conflict

**Cause**: Two DbContexts both trying to run migrations

**Solution**: Use different schemas:
- Burrow.Auth uses `burrow` schema
- Your app should use `public` schema (default)
- Custom modules should use `burrow_{modulename}` schema

---

**Next**: Learn about [Updating Burrow](updating.md) to get new module features
