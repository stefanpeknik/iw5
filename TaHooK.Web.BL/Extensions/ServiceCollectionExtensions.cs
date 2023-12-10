using Microsoft.Extensions.DependencyInjection;
using TaHooK.Web.BL.Installers;

namespace TaHooK.Web.BL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInstaller<TInstaller>(this IServiceCollection services, string apiBaseUrl)
            where TInstaller : WebBLInstaller, new()
        {
            var installer = new TInstaller();
            installer.Install(services, apiBaseUrl);
        }
    }
}
