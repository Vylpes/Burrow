using Microsoft.EntityFrameworkCore;

namespace Example.Web.Data;

public class ExampleDbContext(DbContextOptions<ExampleDbContext> options) : DbContext(options)
{
}
