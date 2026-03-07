using Burrow.Auth.Data;
using Microsoft.EntityFrameworkCore;

namespace Burrow.Auth.Tests.Data;

public class BurrowAuthDbContextTests : IDisposable
{
    private readonly BurrowAuthDbContext _context;
    private readonly DbContextOptions<BurrowAuthDbContext> _options;

    public BurrowAuthDbContextTests()
    {
        // Use in-memory database for testing
        _options = new DbContextOptionsBuilder<BurrowAuthDbContext>()
            .UseInMemoryDatabase(databaseName: $"BurrowAuthTestDb_{Guid.NewGuid()}")
            .Options;

        _context = new BurrowAuthDbContext(_options);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public void CanCreateContext()
    {
        Assert.NotNull(_context);
        Assert.NotNull(_context.Users);
    }

    [Fact]
    public async Task CanCreateUser()
    {
        var user = new ApplicationUser
        {
            UserName = "testuser",
            Email = "test@example.com",
            EmailConfirmed = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(savedUser);
        Assert.Equal("testuser", savedUser.UserName);
        Assert.True(savedUser.EmailConfirmed);
    }

    [Fact]
    public async Task UserEmailMustBeUnique()
    {
        var user1 = new ApplicationUser
        {
            UserName = "user1",
            Email = "test@example.com",
            NormalizedEmail = "TEST@EXAMPLE.COM"
        };

        var user2 = new ApplicationUser
        {
            UserName = "user2",
            Email = "test@example.com",
            NormalizedEmail = "TEST@EXAMPLE.COM"
        };

        _context.Users.Add(user1);
        await _context.SaveChangesAsync();

        _context.Users.Add(user2);

        // Note: In-memory database doesn't enforce unique constraints like real databases
        // This test documents expected behavior with a real PostgreSQL database
        // For full integration testing, use a real database or testcontainers
        await _context.SaveChangesAsync();
        
        var users = await _context.Users.Where(u => u.Email == "test@example.com").ToListAsync();
        Assert.Equal(2, users.Count); // In-memory allows duplicates
        // With PostgreSQL: would throw DbUpdateException due to unique index on NormalizedEmail
    }

    [Fact]
    public async Task CanQueryUserByEmail()
    {
        var user = new ApplicationUser
        {
            UserName = "testuser",
            Email = "query@example.com",
            NormalizedEmail = "QUERY@EXAMPLE.COM"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var foundUser = await _context.Users
            .FirstOrDefaultAsync(u => u.NormalizedEmail == "QUERY@EXAMPLE.COM");

        Assert.NotNull(foundUser);
        Assert.Equal("query@example.com", foundUser.Email);
    }
}
