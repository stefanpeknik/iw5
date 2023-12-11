using System.Security.Claims;

namespace TaHooK.IdentityProvider.BL.Models.AppUser;

public class AppUserExternalCreateModel
{
    public required string? Email { get; set; }
    public required string Provider { get; set; }
    public required string ProviderIdentityKey { get; set; }
    public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
}