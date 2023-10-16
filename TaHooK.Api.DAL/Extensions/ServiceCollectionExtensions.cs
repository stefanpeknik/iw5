using TaHooK.Common.Installers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaHooK.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInstaller<TInstaller>(this IServiceCollection serviceCollection, string connectionString)
            where TInstaller : DALInstaller, new()
        {
            var installer = new TInstaller();
            installer.Install(serviceCollection,connectionString);
        }
    }
}