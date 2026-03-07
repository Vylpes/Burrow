# Burrow Updater Tool

**Status:** Placeholder implementation - architecture designed, full implementation pending.

## Overview

The Burrow Updater is a .NET global tool designed to help downstream projects selectively update their Burrow modules while preserving local customizations.

## Architecture

See [/Burrow/docs/update-tool-architecture.md](../docs/update-tool-architecture.md) for the complete architectural design.

## Current Implementation

This is a **placeholder implementation** demonstrating the command structure and UX. The actual update logic (downloading files, comparing hashes, applying updates) is not yet implemented.

## Planned Commands

```bash
# Check for updates
burrow-updater check

# Show what changed
burrow-updater diff [--version X.Y.Z]

# Apply updates
burrow-updater update [--version X.Y.Z] [--module Auth] [--dry-run] [--force]

# Restore from backup
burrow-updater rollback [--timestamp YYYYMMDD-HHMMSS]
```

## Future Development

When fully implemented, the tool will:
- Download manifests from GitHub releases
- Compare file hashes to detect modifications
- Selectively update files while preserving customizations
- Create automatic backups before updates
- Provide rollback functionality
- Support module-specific updates

## Alternative: Manual Update Process (Current Recommendation)

Until the updater tool is fully implemented, follow this manual process:

### Manual Update Steps

1. **Check current version:**
   ```bash
   cat Burrow/version.txt
   ```

2. **Download new version:**
   ```bash
   # Visit GitHub releases page
   # Download the Burrow folder from the desired release
   ```

3. **Backup current Burrow:**
   ```bash
   cp -r Burrow Burrow.backup-$(date +%Y%m%d)
   ```

4 **Replace selectively:**
   - Keep your `Burrow/src/Burrow.Auth/Data/Migrations/` if you have custom migrations
   - Keep customized email senders or extension methods
   - Replace everything else

5. **Update version file:**
   ```bash
   echo "X.Y.Z" > Burrow/version.txt
   ```

6. **Test:**
   ```bash
   dotnet build
   dotnet test
   ```

## Building from Source

```bash
cd Burrow/tools/Burrow.Updater
dotnet build
dotnet run -- check
```

## Publishing (For Maintainers)

```bash
dotnet pack
dotnet tool install --global --add-source ./nupkg Burrow.Updater
```
