# Updating Burrow

This guide explains how to update your project to a newer version of Burrow while preserving your customizations.

## Overview

Burrow follows semantic versioning (MAJOR.MINOR.PATCH):
- **MAJOR**: Breaking changes (requires migration effort)
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes (safe to update)

Current version: **0.1.0**

## Quick Reference

```bash
# 1. Check your current version
cat Burrow/version.txt

# 2. Backup your Burrow folder
cp -r Burrow Burrow.backup-$(date +%Y%m%d)

# 3. Download new version from GitHub releases

# 4. Replace Burrow folder (except customizations)

# 5. Test
dotnet build && dotnet test

# 6. Update version file
echo "X.Y.Z" > Burrow/version.txt
```

## Update Strategies

### Strategy 1: Full Replacement (Recommended)

**Best for**: Projects that haven't customized Burrow modules

**Process**:
1. Backup current `Burrow/` folder
2. Delete current `Burrow/` folder
3. Copy new `Burrow/` folder from release
4. Restore any custom migrations
5. Test and verify

### Strategy 2: Selective Update

**Best for**: Projects with Burrow customizations

**Process**:
1. Backup current `Burrow/` folder
2. Compare file lists: `diff -qr Burrow/ new-burrow/`
3. Manually copy updated files, skipping customizations
4. Merge changes if needed
5. Test and verify

### Strategy 3: Using the Update Tool (Future)

Once the [Burrow Updater](../tools/Burrow.Updater/README.md) tool is fully implemented:

```bash
burrow-updater check         # Check for updates
burrow-updater diff          # See what changed
burrow-updater update        # Apply updates
```

## Step-by-Step: Full Replacement

### 1. Check Current Version

```bash
cat Burrow/version.txt
```

Output: `0.1.0`

### 2. Check for Updates

Visit the [Burrow releases page](https://git.vylpes.xyz/RabbitLabs/Burrow/releases) and note the latest version.

### 3. Review Release Notes

Read the release notes for breaking changes and new features. Pay attention to:
- **Breaking Changes** section
- **Migration Guide** (if provided)
- **New Features** you might want to use

### 4. Backup Your Project

```bash
# Full project backup
git add -A
git commit -m "Pre-update backup: Burrow v0.1.0 → vX.Y.Z"

# Or just Burrow folder
cp -r Burrow Burrow.backup-$(date +%Y%m%d-%H%M%S)
```

### 5. Note Custom Migrations

If you've created custom migrations in Burrow.Auth:

```bash
ls -la Burrow/src/Burrow.Auth/Data/Migrations/
```

**Important**: Keep your custom migration files! You'll restore them after the update.

### 6. Download New Version

**Option A - GitHub Release**:
1. Go to [Releases](https://github.com/Vylpes/burrow/releases)
2. Download source code for the target version
3. Extract to a temporary directory

**Option B - Git Tag**:
```bash
cd /tmp
git clone --branch vX.Y.Z --depth 1 https://github.com/Vylpes/burrow.git burrow-new
```

### 7. Replace Burrow Folder

```bash
# From your project root
rm -rf Burrow/
cp -r /tmp/burrow-new/Burrow ./
```

### 8. Restore Custom Migrations (If Any)

```bash
# Copy your custom migration files back
cp Burrow.backup-TIMESTAMP/src/Burrow.Auth/Data/Migrations/YYYYMMDD_YourMigration* \
   Burrow/src/Burrow.Auth/Data/Migrations/
```

### 9. Update Version File

The new Burrow folder already has the correct version, but verify:

```bash
cat Burrow/version.txt
# Should show: X.Y.Z
```

### 10. Build and Test

```bash
# Clean build
dotnet clean
dotnet restore
dotnet build

# Run all tests
dotnet test

# Run your application
dotnet run --project src/Example.Web
```

### 11. Test Authentication

1. Register a new account
2. Confirm email
3. Log in
4. Test 2FA (if enabled)
5. Test password reset

### 12. Review Breaking Changes

If the release notes mentioned breaking changes, verify your code still works. Common areas:
- Email configuration format
- Extension method signatures
- Database schema changes

### 13. Commit Updates

```bash
git add Burrow/
git commit -m "Update Burrow: v0.1.0 → vX.Y.Z"
```

## Handling Customizations

### If You Modified Burrow Code

**Problem**: You directly edited files in `Burrow/src/`

**Solution**:
1. Document your changes before updating
2. After update, re-apply changes manually
3. Consider creating a patch file:
   ```bash
   diff -u Burrow/src/Burrow.Auth/Services/Email/EmailSettings.cs \
           Burrow.backup/src/Burrow.Auth/Services/Email/EmailSettings.cs \
           > my-customizations.patch
   ```

**Better approach**: Extend instead of modify:
- Create custom email sender in your app
- Use inheritance or composition
- Keep Burrow code pristine for easy updates

### Preserving Email Customizations

If you customized email templates:

1. **Before update**: Extract your custom templates to `src/YourProject/EmailTemplates/`
2. **Create custom email sender** in your app that loads your templates
3. **Register it** instead of Burrow's default:
   ```csharp
   services.AddSingleton<IEmailSender<ApplicationUser>, CustomEmailSender>();
   ```

### Custom Identity Configuration

If you customized Identity options:

```csharp
// Your code should override Burrow's defaults
builder.Services.Configure<IdentityOptions>(options =>
{
    // Your custom settings
    options.Password.RequireDigit = false;
    // etc.
});
```

This survives updates since it's in `/src/`, not `/Burrow/`.

## Version Compatibility

| Your Version | Can Update To | Notes |
|--------------|---------------|-------|
| 0.1.0        | 0.1.x         | Safe - bug fixes only |
| 0.1.0        | 0.2.0         | Review release notes for new features |
| 0.1.0        | 1.0.0         | Breaking changes - check migration guide |

## Rollback Procedure

If the update causes issues:

### Option 1: Git Rollback

```bash
git log --oneline  # Find commit hash
git revert <commit-hash>
# Or
git reset --hard <pre-update-commit>
```

### Option 2: Manual Rollback

```bash
rm -rf Burrow/
cp -r Burrow.backup-TIMESTAMP Burrow
dotnet clean
dotnet build
```

### Option 3: Using Update Tool (Future)

```bash
burrow-updater rollback --timestamp YYYYMMDD-HHMMSS
```

## Update Checklist

Use this checklist when updating:

- [ ] Read release notes completely
- [ ] Check for breaking changes
- [ ] Backup project (Git commit)
- [ ] Note custom migrations
- [ ] Download new Burrow version
- [ ] Replace Burrow/ folder
- [ ] Restore custom migrations
- [ ] Verify version.txt
- [ ] dotnet clean && dotnet build
- [ ] dotnet test (all tests pass)
- [ ] Manual testing (register, login, etc.)
- [ ] Review deprecated features (if any)
- [ ] Update documentation (if API changed)
- [ ] Commit changes
- [ ] Deploy to staging
- [ ] Test in staging
- [ ] Deploy to production

## Automated Updates (Future)

The Burrow Update Tool is planned to automate this process:

- Automatically detect changes
- Show what will be updated
- Preserve customizations
- Create automatic backups
- Handle migrations intelligently
- Provide rollback option

See [Update Tool Architecture](update-tool-architecture.md) for details.

## Getting Help with Updates

If you encounter issues:

1. **Check GitHub Issues**: Search for similar problems
2. **Review migration guide**: In release notes
3. **Ask in Discussions**: Post your question with:
   - Current version
   - Target version
   - Error messages
   - Steps you've tried
4. **Open an issue**: If you found a bug

## Best Practices

1. **Update regularly**: Don't skip multiple versions
2. **Test thoroughly**: Use a staging environment
3. **Read release notes**: Don't skip this step
4. **Keep Burrow pristine**: Don't modify Burrow code directly
5. **Use Git**: Always commit before updating
6. **Automate testing**: CI/CD helps catch update issues early

---

**Need help?** See [Getting Started](getting-started.md) or [Module Management](modules.md)
