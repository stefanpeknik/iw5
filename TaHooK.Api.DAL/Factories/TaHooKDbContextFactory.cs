using Microsoft.EntityFrameworkCore;

namespace TaHooK.Api.DAL.Factories;

public class TaHooKDbContextFactory: IDbContextFactory<TaHooKDbContext>
{
    private readonly string _connectionString;
    private readonly bool _seedData;

    public TaHooKDbContextFactory(string connectionString, bool seedData = false)
    {
        _connectionString = connectionString;
        _seedData = seedData;
    }
    
    public TaHooKDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaHooKDbContext>();
        optionsBuilder.UseSqlServer(_connectionString);

        var context = new TaHooKDbContext(optionsBuilder.Options, _seedData);

        return context;
    }
}