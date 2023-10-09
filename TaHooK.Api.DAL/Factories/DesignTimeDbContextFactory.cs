using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaHooK.Api.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaHooKDbContext>
{
    public TaHooKDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<DesignTimeDbContextFactory>(optional: true)
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<TaHooKDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new TaHooKDbContext(optionsBuilder.Options);
    }
}