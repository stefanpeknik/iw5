using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Common.Entities;

namespace TaHooK.Api.Common.Tests.Seeds
{
    public static class QuestionSeeds
    {

        public static readonly QuestionEntity DefaultQuestion = new()
        {
            Id = Guid.Parse("574E8D70-47A1-4D76-B971-CAFB314540FD"),
            Quiz = QuizSeeds.DefaultQuiz,
            QuizId = Guid.Parse("EF2E391C-EA09-490B-9935-BBC7E7099A42"),
            Text = "Test Question"
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerEntity>().HasData(
                DefaultQuestion with { Answers = Array.Empty<AnswerEntity>() }
            );
        }
    }
}