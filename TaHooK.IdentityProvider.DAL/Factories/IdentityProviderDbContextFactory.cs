using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaHooK.IdentityProvider.DAL.Factories;

public class IdentityProviderDbContextFactory : IDesignTimeDbContextFactory<IdentityProviderDbContext>, IDbContextFactory<IdentityProviderDbContext>
{
    private readonly string _connectionString;

    public IdentityProviderDbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IdentityProviderDbContext CreateDbContext(string[] args)
        => CreateDbContext();

    public IdentityProviderDbContext CreateDbContext()
    {
        
        var optionsBuilder = new DbContextOptionsBuilder<IdentityProviderDbContext>();
        optionsBuilder.UseSqlServer(_connectionString);
        return new IdentityProviderDbContext(optionsBuilder.Options);
    }
}
