using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using TaHooK.IdentityProvider.BL.Facades.Interfaces;

namespace TaHooK.IdentityProvider.App.Services;

public class LocalAppUserProfileService : IProfileService
{
    private readonly IAppUserFacade appUserFacade;

    public LocalAppUserProfileService(
        IAppUserFacade appUserFacade)
    {
        this.appUserFacade = appUserFacade;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userName = context.Subject.GetSubjectId();
        var user = await appUserFacade.GetUserByUserNameAsync(userName);
        if (user is not null)
        {
            var claims = new List<Claim>
            {
                new (nameof(user.Id), user.Id.ToString()),
                new (nameof(user.Email), user.Email),
                new (nameof(user.DisplayName), user.DisplayName)
            };
            context.RequestedClaimTypes = claims.Select(claim => claim.Type);
            context.AddRequestedClaims(claims);
        }

    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
    }
}
