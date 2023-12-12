namespace TaHooK.IdentityProvider.BL.Models.AppUser;

public class AppUserDetailModel
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
}
