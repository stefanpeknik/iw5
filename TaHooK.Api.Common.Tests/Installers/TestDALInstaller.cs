using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaHooK.Api.Common.Tests.Factories;
using TaHooK.Api.DAL;
using TaHooK.Api.DAL.Migrators;
using TaHooK.Api.DAL.Repositories;
using TaHooK.Common.Installers;

namespace TaHooK.Api.Common.Tests.Installers;

public class TestDALInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDbContextFactory<TaHooKDbContext>>(provider =>
            new DbContextTestingFactory(Guid.NewGuid().ToString(), true));
        serviceCollection.AddSingleton<IDbMigrator, TestDbMigrator>();

        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<TestDALInstaller>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                .AsMatchingInterface()
                .WithScopedLifetime());
    }
}