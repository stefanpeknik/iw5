using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaHooK.Api.DAL.Common.Entities;

namespace TaHooK.Api.Common.Tests.Seeds
{
    public static class ScoreSeeds
    {

        public static readonly ScoreEntity DefaultScore = new()
        {
            Id = Guid.Parse("4740A96E-76D0-4815-9094-9FBFD3190F30"),
            Quiz = null!,
            QuizId = QuizSeeds.DefaultQuiz.Id,
            Score = 10,
            User = null!,
            UserId = UserSeeds.DefaultUser.Id
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScoreEntity>().HasData(
                DefaultScore
            );
        }
    }
}
