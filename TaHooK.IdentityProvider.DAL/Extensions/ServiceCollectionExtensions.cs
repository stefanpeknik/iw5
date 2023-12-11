using Microsoft.Extensions.DependencyInjection;
using TaHooK.IdentityProvider.DAL.Installers;

namespace TaHooK.IdentityProvider.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInstaller<TInstaller>(this IServiceCollection serviceCollection, string connectionString)
        where TInstaller : IdentityProviderDALInstaller, new()
    {
        var installer = new TInstaller();
        installer.Install(serviceCollection, connectionString);
    }
}