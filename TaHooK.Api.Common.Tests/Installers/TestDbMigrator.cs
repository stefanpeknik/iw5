using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL;
using TaHooK.Api.DAL.Migrators;

namespace TaHooK.Api.Common.Tests.Installers;

public class TestDbMigrator : IDbMigrator
{
    private readonly IDbContextFactory<TaHooKDbContext> _dbContextFactory;

    public TestDbMigrator(IDbContextFactory<TaHooKDbContext> dbContextFactory)
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

        // If you want to delete the database before migration, uncomment the following line
        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}