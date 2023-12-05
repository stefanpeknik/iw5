using TaHooK.Common.BL.Facades;
using TaHooK.IdentityProvider.BL.Models;

namespace TaHooK.IdentityProvider.BL.Facades;

public interface IAppUserClaimsFacade : IAppFacade
{
    Task<IEnumerable<AppUserClaimListModel>> GetAppUserClaimsByUserIdAsync(Guid userId);
}
