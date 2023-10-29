using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.Common.Tests.Seeds
{
    public static class UserSeeds
    {

        public static readonly UserEntity DefaultUser = new()
        {
            Id = Guid.Parse("A7F6F50A-3B1A-4065-8274-62EDD210CD1A"),
            Email = "ferda@gmail.com",
            Name = "Ferda",
            Photo = new Uri("https://www.merchandising.cz/images/F01_2020_i.jpg")
        };
        
        public static readonly UserEntity UserToDelete = DefaultUser with { Id = Guid.Parse("8C43E708-44CA-4D7A-9F76-5D2A2155BF02") };
        
        public static readonly UserEntity UserToUpdate = DefaultUser with { Id = Guid.Parse("267FA2C2-D406-4E22-9AA9-4EC927E6F1CF") };
        

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(
                DefaultUser with{ Scores = Array.Empty<ScoreEntity>() },
                UserToDelete with{ Scores = Array.Empty<ScoreEntity>() },
                UserToUpdate with{ Scores = Array.Empty<ScoreEntity>() }
            );
        }
    }
}
