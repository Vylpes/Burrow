# Burrow

**A modular ASP.NET Core template framework for building web applications with pre-built authentication, ready to clone and customize.**

Version: 0.1.0

## Overview

Burrow is a template repository that provides a solid foundation for ASP.NET Core 8 web applications. It includes:

- ✅ **Modular Authentication** ([Burrow.Auth](Burrow/src/Burrow.Auth/)) - Complete ASP.NET Core Identity with Entity Framework, PostgreSQL schema isolation, 50+ Razor pages, and configurable email senders
- ✅ **Example Project** ([Example.Web](src/Example.Web/)) - Working Blazor Server app demonstrating integration
- ✅ **Docker Setup** - PostgreSQL database with docker-compose
- ✅ **Production Ready** - Comprehensive test coverage, validation, error handling

### Key Design Principles

1. **Modular**: Only include what you need (e.g., add Burrow.Auth only if you need authentication)
2. **Updatable**: All template code lives in `/Burrow/` folder for easy downstream updates
3. **Customizable**: Project-specific code lives in `/src/` - modify without breaking updates
4. **Well-Documented**: Complete guides for setup, usage, architecture, and contributions

## Quick Start

```bash
# 1. Create new repository from this template on GitHub
# 2. Clone your new repository
git clone https://github.com/YOUR_USERNAME/YOUR_PROJECT.git
cd YOUR_PROJECT

# 3. Start PostgreSQL database
docker-compose up -d

# 4. Configure connection string (if needed)
# Edit src/Example.Web/appsettings.Development.json

# 5. Run the application
dotnet run --project src/Example.Web

# 6. Open https://localhost:7164
# 7. Register a new account (check .temp/emails/ for confirmation email)
```

## Documentation

**Get Started:**
- [Getting Started Guide](Burrow/docs/getting-started.md) - Detailed setup instructions
- [Quick Start for New Projects](Burrow/docs/quickstart-new-project.md) - Create from template

**Usage:**
- [Module Management](Burrow/docs/modules.md) - Add/remove Burrow modules
- [Updating Burrow](Burrow/docs/updating.md) - How to pull template updates

**Architecture:**
- [Architecture Overview](Burrow/docs/architecture/overview.md) - Project structure and design goals
- [Authentication Design](Burrow/docs/architecture/authentication.md) - Burrow.Auth internals
- [Modularity](Burrow/docs/architecture/modularity.md) - How modules stay independent
- [Database Patterns](Burrow/docs/architecture/database-patterns.md) - Schema separation strategy

**Development:**
- [Contributing Guide](Burrow/docs/CONTRIBUTING.md) - Develop Burrow itself
- [Customization Examples](Burrow/docs/examples/) - Extend and customize

## Project Structure

```
├── Burrow/                       # Template framework (keep for updates)
│   ├── src/
│   │   └── Burrow.Auth/          # Authentication module
│   ├── tests/
│   │   └── Burrow.Auth.Tests/    # Unit & integration tests
│   ├── tools/
│   │   └── Burrow.Updater/       # Update tool (planned)
│   ├── docs/                     # Documentation
│   └── version.txt               # Current Burrow version
│
├── src/                          # Your project code (customize freely)
│   └── Example.Web/              # Example Blazor Server app
│
├── docker-compose.yml            # PostgreSQL setup
└── README.md                     # This file
```

For detailed documentation, see [Getting Started Guide](Burrow/docs/getting-started.md).


