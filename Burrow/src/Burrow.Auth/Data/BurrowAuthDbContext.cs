using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Burrow.Auth.Data;

public class BurrowAuthDbContext : IdentityDbContext<ApplicationUser>
{
    // Accept the non-generic base type so derived contexts (e.g. DropletDbContext)
    // can pass their own DbContextOptions<T> via the inheritance chain.
    public BurrowAuthDbContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("AspNetUsers", "burrow");
        builder.Entity<IdentityRole>().ToTable("AspNetRoles", "burrow");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", "burrow");
        builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims", "burrow");
        builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins", "burrow");
        builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", "burrow");
        builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens", "burrow");
    }
}
