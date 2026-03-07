# Burrow Update Tool Architecture

## Overview

The Burrow Update Tool enables downstream projects to selectively pull updates from the Burrow template repository while preserving local customizations.

## Design Goals

1. **Simple**: Easy to understand and use without deep Git knowledge
2. **Selective**: Allow updating specific modules or files
3. **Safe**: Prevent accidental overwrite of customizations
4. **Cross-platform**: Work on Windows, Linux, and macOS
5. **Minimal dependencies**: Should work with just .NET SDK installed

## Architecture Decision: .NET Global Tool

**Chosen Approach**: Implement as a .NET Global Tool

### Rationale

- Cross-platform (Windows, Linux, macOS) with single codebase
- No shell script duplication (bash + PowerShell)
- Can be versioned and distributed via NuGet
- Familiar installation: `dotnet tool install -g Burrow.Updater`
- Access to full .NET ecosystem for HTTP, file I/O, JSON parsing

### Alternative Approaches Considered

1. **Bash + PowerShell scripts**: Rejected due to maintenance of two codebases
2. **Git-based (git remote + merge)**: Rejected as too complex for users
3. **Node.js tool**: Rejected to avoid Node.js dependency

## Update Mechanism

### Version Tracking

```
/Burrow/version.txt
```
Contains semantic version: `0.1.0`

### Manifest File

```
/Burrow/.burrow-manifest.json
```

```json
{
  "version": "0.1.0",
  "modules": {
    "Burrow.Auth": {
      "path": "src/Burrow.Auth",
      "files": [
        {
          "path": "Burrow.Auth.csproj",
          "hash": "sha256:abc123...",
          "userCustomizable": false
        },
        {
          "path": "Extensions/BurrowAuthExtensions.cs",
          "hash": "sha256:def456...",
          "userCustomizable": false
        }
      ],
      "excludeFromUpdate": [
        "Data/Migrations/*.cs"
      ]
    }
  }
}
```

### Update Process

1. **Check current version**
   ```
   burrow-updater check
   ```
   - Reads local `/Burrow/version.txt`
   - Fetches latest version from template repository
   - Shows available updates

2. **List changes**
   ```
   burrow-updater diff [version]
   ```
   - Downloads manifest for target version
   - Compares file hashes
   - Lists: added, modified, deleted files
   - Marks files with local modifications

3. **Update**
   ```
   burrow-updater update [--version X.Y.Z] [--module Auth] [--dry-run]
   ```
   - Downloads files from template repository
   - Updates only unchanged files by default
   - Flags: 
     - `--force`: Overwrite modified files (prompted per file)
     - `--module`: Update specific module only
     - `--dry-run`: Show what would be updated without applying
   - Creates backup: `/Burrow/.backup/YYYYMMDD-HHMMSS/`
   - Updates `/Burrow/version.txt` on success

4. **Rollback**
   ```
   burrow-updater rollback [backup-timestamp]
   ```
   - Restores from backup directory
   - Lists available backups if timestamp not provided

### File Categorization

**Never Updated Automatically**:
- User migration files: `Data/Migrations/*.cs` (except Burrow's own)
- Custom email senders in downstream projects
- Any file with `// CUSTOMIZED` comment marker

**Safe to Update**:
- Core library code
- Razor components
- CSS/JS assets
- Configuration templates (with warning if modified)

**Prompt Before Update**:
- Extension methods (if customized)
- Middleware setup files
- appsettings templates

## Implementation Plan

### Tool Structure

```
Burrow.Updater/
├── Burrow.Updater.csproj
├── Program.cs                 # CLI entry point
├── Commands/
│   ├── CheckCommand.cs       # Check for updates
│   ├── DiffCommand.cs        # Show differences
│   ├── UpdateCommand.cs      # Apply updates
│   └── RollbackCommand.cs    # Restore from backup
├── Services/
│   ├── ManifestService.cs    # Load/compare manifests
│   ├── DownloadService.cs    # Fetch files from template
│   ├── HashService.cs        # Calculate SHA256 hashes
│   └── BackupService.cs      # Create/restore backups
└── Models/
    ├── BurrowManifest.cs
    └── UpdatePlan.cs
```

### Technology Stack

- **CLI Framework**: System.CommandLine
- **HTTP**: HttpClient
- **JSON**: System.Text.Json
- **Hashing**: System.Security.Cryptography
- **Output**: Spectre.Console (for nice terminal UI)

### Template Repository Requirements

1. **GitHub Releases**: Each Burrow version published as GitHub release
2. **Manifest generation**: CI/CD generates `.burrow-manifest.json` on release
3. **Version file**: `Burrow/version.txt` updated on each release

## Usage Examples

### Typical Workflow

```bash
# Check if updates available
$ burrow-updater check
Current version: 0.1.0
Latest version: 0.2.0
Update available! Run 'burrow-updater diff' to see changes.

# Review changes
$ burrow-updater diff
Changes from 0.1.0 to 0.2.0:
Added:
  - Burrow/src/Burrow.Auth/Services/TwoFactor/TotpService.cs
Modified:
  - Burrow/src/Burrow.Auth/Extensions/BurrowAuthExtensions.cs
  - Burrow/src/Burrow.Auth/Burrow.Auth.csproj

Warning: Modified files detected:
  - Burrow/src/Burrow.Auth/Services/Email/EmailSettings.cs (customized locally)

# Apply update
$ burrow-updater update
Creating backup: Burrow/.backup/20260307-143022/
Downloading updated files...
Updated 15 files
Skipped 1 customized file:
  - Burrow/src/Burrow.Auth/Services/Email/EmailSettings.cs
    Use --force to overwrite (will prompt)

Update complete! 0.1.0 → 0.2.0
```

### Update Only Specific Module

```bash
$ burrow-updater update --module Auth
Updating only Burrow.Auth module...
```

### Force Update with Prompts

```bash
$ burrow-updater update --force
? File 'EmailSettings.cs' was customized. Overwrite? (y/N) n
  Kept your version
? File 'BurrowAuthExtensions.cs' was modified. Overwrite? (y/N) y
  Updated from template
```

## Security Considerations

- **Source validation**: Only download from official GitHub repository
- **Integrity**: Verify downloaded file hashes match manifest
- **Backups**: Always create backup before modifying files
- **Transparency**: Show exactly what will be changed before applying

## Future Enhancements

1. **Conflict resolution UI**: Interactive 3-way merge for conflicted files
2. **Patch mode**: Generate and apply git-style patches
3. **VS Code extension**: Visual diff and update from within editor
4. **Update notifications**: Check for updates on build (warning only)
5. **Selective file updates**: Update individual files instead of whole modules
