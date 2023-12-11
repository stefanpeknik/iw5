using Microsoft.EntityFrameworkCore;

namespace TaHooK.IdentityProvider.DAL.Migrators;

public class SqlDbMigrator : IDbMigrator
{
    private readonly IDbContextFactory<IdentityProviderDbContext> _dbContextFactory;

    public SqlDbMigrator(IDbContextFactory<IdentityProviderDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public void Migrate()
    {
        MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}
