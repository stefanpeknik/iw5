using TaHooK.Common.BL.Facades;
using TaHooK.Common.Installers;
using TaHooK.IdentityProvider.BL.MapperProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace TaHooK.IdentityProvider.BL.Installers;

public class IdentityProviderBLInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(AppUserMapperProfile));

        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<IdentityProviderBLInstaller>()
                .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
    }
}
