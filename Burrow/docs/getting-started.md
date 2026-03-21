# Getting Started with Burrow

This guide will help you set up a new project using the Burrow template framework.

## Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 8.0 SDK** or later - [Download](https://dotnet.microsoft.com/download)
- **Docker & Docker Compose** - [Download](https://www.docker.com/)
- **Git** - [Download](https://git-scm.com/)

Optional (for Burrow template development):
- **Node.js 18+** and **Yarn** - Only if you need to modify Tailwind CSS styles

## Step 1: Create Your Project

### Option A: Use as GitHub Template

1. Click "Use this template" button on GitHub
2. Enter your repository name
3. Clone your new repository:
   ```bash
   git clone https://github.com/YOUR_USERNAME/YOUR_PROJECT.git
   cd YOUR_PROJECT
   ```

### Option B: Clone Directly

```bash
git clone https://github.com/Vylpes/burrow.git your-project-name
cd your-project-name
git remote remove origin
git remote add origin https://github.com/YOUR_USERNAME/YOUR_PROJECT.git
```

## Step 2: Start the Database

Burrow uses PostgreSQL with Docker:

```bash
docker-compose up -d
```

This starts:
- PostgreSQL 16 on port 5432
- Database name: `burrow`
- Username: `burrow`
- Password: `burrow` (⚠️ change in production!)

**Verify it's running:**
```bash
docker-compose ps
```

## Step 3: Configure Your Application

### Connection String

The default development configuration works out-of-the-box. To customize, edit `src/Example.Web/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=burrow;Username=burrow;Password=burrow"
  }
}
```

### Email Configuration

By default, Burrow uses **file-based email** (emails saved to `.temp/emails/`). This is perfect for development.

For production, configure SMTP in `src/Example.Web/appsettings.json`:

```json
{
  "Email": {
    "Sender": "Smtp",
    "Smtp": {
      "Host": "smtp.example.com",
      "Port": 587,
      "Username": "your-email@example.com",
      "Password": "your-smtp-password",
      "EnableSsl": true,
      "FromAddress": "noreply@yourapp.com",
      "FromName": "Your App Name"
    }
  }
}
```

**Email sender options:**
- `File` - Development (saves to disk)
- `Smtp` - Production (sends real emails)
- `NoOp` - Testing (does nothing)

## Step 4: Build and Run

```bash
# Restore packages and build
dotnet restore
dotnet build

# Run the Example.Web project
dotnet run --project src/Example.Web
```

Or use Visual Studio / Rider and press F5.

The application will start on:
- HTTP: http://localhost:5271
- HTTPS: https://localhost:7164

## Step 5: Test Authentication

1. **Open the app**: Navigate to https://localhost:7164
2. **Register**: Click "Register" and create an account
3. **Check email**: Open `.temp/emails/` folder to find your confirmation email
4. **Copy link**: Open the email file and copy the confirmation URL
5. **Confirm**: Paste the URL in your browser to confirm your account
6. **Login**: Now you can log in with your credentials

## Step 6: Verify Everything Works

Run the test suite:

```bash
dotnet test
```

You should see all tests passing:
```
Test summary: total: 49, failed: 0, succeeded: 49, skipped: 0
```

## Project Structure Overview

```
your-project/
├── Burrow/                      # Template framework (DON'T modify directly)
│   ├── src/Burrow.Auth/         # Authentication module
│   ├── tests/                   # Tests for Burrow modules
│   ├── docs/                    # This documentation
│   └── version.txt              # Burrow version (0.1.0)
│
├── src/                         # YOUR application code
│   └── Example.Web/             # Rename this to your project name
│       ├── Components/          # Blazor components
│       ├── Data/                # Your DbContext
│       └── Program.cs           # Entry point
│
├── docker-compose.yml           # Database setup
├── .temp/                       # Development files (gitignored)
│   └── emails/                  # File-based emails land here
└── README.md
```

## Next Steps

Now that you have Burrow running:

1. **Customize Example.Web**: Rename it to your project name
2. **Add your features**: Create components, pages, and services in `src/`
3. **Extend the database**: Add entities to `src/Example.Web/Data/ExampleDbContext.cs`
4. **Learn the architecture**: Read [Architecture Overview](architecture/overview.md)
5. **Manage modules**: See [Module Management](modules.md)

## Common Issues

### Database Connection Failed

**Problem**: `Npgsql.NpgsqlException: Connection refused`

**Solution**:
1. Check Docker is running: `docker ps`
2. Restart containers: `docker-compose restart`
3. Check logs: `docker-compose logs postgres`

### Port Already in Use

**Problem**: `Address already in use 127.0.0.1:5271`

**Solution**:
1. Change ports in `src/Example.Web/Properties/launchSettings.json`:
   ```json
   "applicationUrl": "https://localhost:7164;http://localhost:5271"
   ```

### Migrations Failed

**Problem**: `A connection was successfully established... but then an error occurred`

**Solution**:
1. Drop the database: `docker-compose down -v`
2. Recreate: `docker-compose up -d`
3. Restart the app (auto-migration will run)

## Development Workflow

### Making Database Changes

```bash
# In Burrow.Auth (if modifying authentication)
cd Burrow/src/Burrow.Auth
dotnet ef migrations add YourMigrationName
dotnet ef database update

# In Example.Web (for your app tables)
cd src/Example.Web
dotnet ef migrations add YourMigrationName --context ExampleDbContext
dotnet ef database update --context ExampleDbContext
```

### Modifying Burrow.Auth Styles

Only needed if you're customizing the authentication UI:

```bash
cd Burrow/src/Burrow.Auth
yarn install
yarn css:watch  # Auto-rebuild CSS on changes
```

## Getting Help

- **Documentation**: Check [docs/](.) for guides
- **Issues**: [GitHub Issues](https://github.com/Vylpes/burrow/issues)
- **Discussions**: [GitHub Discussions](https://github.com/Vylpes/burrow/discussions)

## What's Next?

- [Module Management](modules.md) - Add or remove Burrow modules
- [Architecture Overview](architecture/overview.md) - Understand Burrow's design
- [Customization Examples](examples/) - Common modifications
- [Contributing](CONTRIBUTING.md) - Improve Burrow itself

---

**Need a quick reference?** See the [Quick Start](../README.md#quick-start) in the main README.
