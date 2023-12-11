using TaHooK.Common.Installers;
using TaHooK.IdentityProvider.DAL.Entities;
using TaHooK.IdentityProvider.DAL.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaHooK.IdentityProvider.DAL.Migrators;

namespace TaHooK.IdentityProvider.DAL.Installers;

public class IdentityProviderDALInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDbContextFactory<IdentityProviderDbContext>, IdentityProviderDbContextFactory>();
        serviceCollection.AddSingleton<IDbMigrator, SqlDbMigrator>();
        serviceCollection.AddScoped<IUserStore<AppUserEntity>, UserStore<AppUserEntity, AppRoleEntity, IdentityProviderDbContext, Guid, AppUserClaimEntity, AppUserRoleEntity, AppUserLoginEntity, AppUserTokenEntity, AppRoleClaimEntity>>();
        serviceCollection.AddScoped<IRoleStore<AppRoleEntity>, RoleStore<AppRoleEntity, IdentityProviderDbContext, Guid, AppUserRoleEntity, AppRoleClaimEntity>>();

        serviceCollection.AddTransient(serviceProvider =>
        {
            var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<IdentityProviderDbContext>>();
            return dbContextFactory.CreateDbContext();
        });
    }
}
