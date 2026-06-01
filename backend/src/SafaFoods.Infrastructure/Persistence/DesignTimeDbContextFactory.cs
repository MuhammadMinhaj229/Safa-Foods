using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SafaFoods.Infrastructure.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SafaFoodsDbContext>
{
    public SafaFoodsDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SafaFoodsDbContext>();
        var connectionString =
            Environment.GetEnvironmentVariable("SAFA_FOODS_POSTGRES") ??
            "Host=localhost;Port=5432;Database=safafoods;Username=postgres;Password=your-password";

        builder.UseNpgsql(connectionString, npgsqlOptions =>
            npgsqlOptions.EnableRetryOnFailure());

        return new SafaFoodsDbContext(builder.Options);
    }
}
