using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.DAL.Seeds;

public static class QuestionSeeds
{
    public static readonly QuestionEntity DefaultQuestion = new()
    {
        Id = Guid.Parse("574E8D70-47A1-4D76-B971-CAFB314540FD"),
        QuizTemplate = null!,
        QuizTemplateId = QuizTemplateSeeds.DefaultQuiz.Id,
        Text = "Test Question"
    };

    public static readonly QuestionEntity DefaultQuestion2 = new()
    {
        Id = Guid.Parse("5E1E5547-9526-4FDA-9673-DD6135822CF2"),
        QuizTemplate = null!,
        QuizTemplateId = QuizTemplateSeeds.DefaultQuiz2.Id,
        Text = "Test Question 2"
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuestionEntity>().HasData(
            DefaultQuestion with { Answers = Array.Empty<AnswerEntity>() },
            DefaultQuestion2 with { Answers = Array.Empty<AnswerEntity>() }
        );
    }
}