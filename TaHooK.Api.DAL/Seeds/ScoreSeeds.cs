using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.DAL.Seeds
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
        
        public static readonly ScoreEntity DefaultScore2 = new()
        {
            Id = Guid.Parse("045CEAC7-1440-4778-A134-F75EE91A45D3"),
            Quiz = null!,
            QuizId = QuizSeeds.DefaultQuiz.Id,
            Score = 10,
            User = null!,
            UserId = UserSeeds.DefaultUser2.Id
        };

        public static readonly ScoreEntity DefaultScore3 = new()
        {
            Id = Guid.Parse("CC26A3AD-038B-4EEB-8C26-F01A64125B95"),
            Quiz = null!,
            QuizId = QuizSeeds.DefaultQuiz2.Id,
            Score = 20,
            User = null!,
            UserId = UserSeeds.DefaultUser.Id
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScoreEntity>().HasData(
                DefaultScore,
                DefaultScore2,
                DefaultScore3
            );
        }
    }
}
