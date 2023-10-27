

using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using TaHooK.Common.Installers;

namespace TaHooK.Api.BL.Installers;

public class BlInstaller: IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        
        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<BlInstaller>()
                .AddClasses(classes => classes.AssignableTo(typeof(IFacade)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
    }
}