using Microsoft.EntityFrameworkCore;

namespace TaHooK.Api.DAL.Migrators;

public class SqlDbMigrator: IDbMigrator
{
    private readonly IDbContextFactory<TaHooKDbContext> _dbContextFactory;

    public SqlDbMigrator(IDbContextFactory<TaHooKDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public void Migrate() => MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using TaHooKDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        // If you want to delete the database before migration, uncomment the following line
        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}