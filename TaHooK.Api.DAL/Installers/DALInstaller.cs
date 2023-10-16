namespace TaHooK.Api.DAL.Installers;
using TaHooK.Api.DAL.Factories;
using TaHooK.Api.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaHooK.Common.Installers;

public class DALInstaller
{
    public void Install(IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddSingleton<IDbContextFactory<TaHooKDbContext>>(provider => new TaHooKDbContextFactory(connectionString, true));
        serviceCollection.AddSingleton<IDbMigrator, SqlDbMigrator>();

        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<DALInstaller>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                .AsMatchingInterface()
                .WithScopedLifetime());
    }
}