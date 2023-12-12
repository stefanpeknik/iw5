using Microsoft.AspNetCore.Identity;
using TaHooK.IdentityProvider.DAL.Entities;

namespace TaHooK.IdentityProvider.DAL.Seeds;

public class AppUserSeeds
{
    private readonly UserManager<AppUserEntity> _userManager;

    public AppUserSeeds(UserManager<AppUserEntity> userManager)
    {
        _userManager = userManager;
    }
    
    public static readonly AppUserEntity DefaultUser = new()
    {
        Id = Guid.Parse("A7F6F50A-3B1A-4065-8274-62EDD210CD1A"),
        Subject = "Ferda",
        UserName = "Ferda",
        DisplayName = "Ferda",
        Email = "ferda@gmail.com",
    };

    public static readonly AppUserEntity DefaultUser2 = new()
    {
        Id = Guid.Parse("14B1DEFA-2350-46C9-9C1C-01E4C63ACD79"),
        Subject = "broukpytlik",
        UserName = "broukpytlik",
        DisplayName = "broukpytlik",
        Email = "broukpytlik@gmail.com"
    };
    
public async Task SeedAsync()
{
        var users = new List<AppUserEntity>
        {
            DefaultUser,
            DefaultUser2
        };
        
        foreach (var user in users)
        {
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser != null)
            {
                continue;
            }
            await _userManager.CreateAsync(user, "test123");
        }
    }
}