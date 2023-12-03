using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity DefaultUser = new()
    {
        Id = Guid.Parse("A7F6F50A-3B1A-4065-8274-62EDD210CD1A"),
        Email = "ferda@gmail.com",
        Name = "Ferda",
        Photo = new Uri("https://www.merchandising.cz/images/F01_2020_i.jpg")
    };

    public static readonly UserEntity DefaultUser2 = new()
    {
        Id = Guid.Parse("14B1DEFA-2350-46C9-9C1C-01E4C63ACD79"),
        Email = "broukpytlik@gmail.com",
        Name = "Brouk Pytlík",
        Photo = new Uri("https://vltava.rozhlas.cz/sites/default/files/images/00865327.jpeg")
    };
    
    public static IEnumerable<UserEntity> GetDefaultUsers()
    {
        return new List<UserEntity>()
        {
            DefaultUser with { Scores = new List<ScoreEntity>() },
            DefaultUser2 with { Scores = new List<ScoreEntity>() }
        };
    }
}