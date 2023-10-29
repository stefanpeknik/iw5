using Microsoft.Extensions.DependencyInjection;

namespace TaHooK.Common.Installers;

public interface IInstaller
{
    void Install(IServiceCollection serviceCollection);
}