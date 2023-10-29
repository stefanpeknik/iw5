using Microsoft.Extensions.DependencyInjection;
using TaHooK.Api.DAL.Installers;

namespace TaHooK.Api.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInstaller<TInstaller>(this IServiceCollection serviceCollection, string connectionString)
        where TInstaller : DALInstaller, new()
    {
        var installer = new TInstaller();
        installer.Install(serviceCollection, connectionString);
    }
}