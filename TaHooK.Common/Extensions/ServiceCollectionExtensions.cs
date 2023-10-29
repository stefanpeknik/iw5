using Microsoft.Extensions.DependencyInjection;
using TaHooK.Common.Installers;

namespace TaHooK.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInstaller<TInstaller>(this IServiceCollection serviceCollection)
        where TInstaller : IInstaller, new()
    {
        var installer = new TInstaller();
        installer.Install(serviceCollection);
    }
}