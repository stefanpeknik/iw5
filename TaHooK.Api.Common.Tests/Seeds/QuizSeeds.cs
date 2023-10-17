using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.Common.Tests.Seeds
{
    public static class QuizSeeds
    {

        public static readonly QuizEntity DefaultQuiz = new()
        {
            Id = Guid.Parse("EF2E391C-EA09-490B-9935-BBC7E7099A42"),
            Schedule = new DateTime(2023, 5, 5),
            Title = "Fun Trivia",
            Finished = true
        };
        
        public static readonly QuizEntity QuizToDelete = DefaultQuiz with { Id = Guid.Parse("B01885A7-F139-4597-95C6-2118649EB3A5") };
        
        public static readonly QuizEntity QuizToUpdate = DefaultQuiz with { Id = Guid.Parse("C2C75AF2-1262-464F-83B4-05397C9D36C7") };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuizEntity>().HasData(
                DefaultQuiz with { Questions = Array.Empty<QuestionEntity>(), Scores = Array.Empty<ScoreEntity>() },
                QuizToDelete with { Questions = Array.Empty<QuestionEntity>(), Scores = Array.Empty<ScoreEntity>() },
                QuizToUpdate with { Questions = Array.Empty<QuestionEntity>(), Scores = Array.Empty<ScoreEntity>() }
            );
        }
    }
}