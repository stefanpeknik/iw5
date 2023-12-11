namespace TaHooK.IdentityProvider.DAL.Migrators;

public interface IDbMigrator
{
    public void Migrate(bool isDev);
    public Task MigrateAsync(CancellationToken cancellationToken, bool isDev);
}