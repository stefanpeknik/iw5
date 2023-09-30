using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaHooK.Api.DAL.Common.Entities;

namespace TaHooK.Api.Common.Tests.Seeds
{
    public static class UserSeeds
    {

        public static readonly UserEntity DefaultUser = new()
        {
            Id = Guid.Parse("A7F6F50A-3B1A-4065-8274-62EDD210CD1A"),
            Email = "ferda@gmail.com",
            Name = "Ferda",
            Password = "Ferda123",
            Photo = null!
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(
                DefaultUser with{ Scores = Array.Empty<ScoreEntity>() }
            );
        }
    }
}
