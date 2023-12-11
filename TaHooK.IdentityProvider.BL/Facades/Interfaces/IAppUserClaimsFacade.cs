using TaHooK.Common.BL.Facades;
using TaHooK.IdentityProvider.BL.Models.AppUserClaim;

namespace TaHooK.IdentityProvider.BL.Facades.Interfaces;

public interface IAppUserClaimsFacade : IAppFacade
{
    Task<IEnumerable<AppUserClaimListModel>> GetAppUserClaimsByUserIdAsync(Guid userId);
}
