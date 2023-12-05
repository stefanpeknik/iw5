using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.Common.Tests.Seeds;

public static class QuestionSeeds
{
    public static readonly QuestionEntity DefaultQuestion = new()
    {
        Id = Guid.Parse("574E8D70-47A1-4D76-B971-CAFB314540FD"),
        QuizTemplate = null!,
        QuizTemplateId = Guid.Parse("EF2E391C-EA09-490B-9935-BBC7E7099A42"),
        Text = "Test Question"
    };

    public static readonly QuestionEntity QuestionToDelete =
        DefaultQuestion with { Id = Guid.Parse("85A30A08-201D-4BD2-8804-A5EDCD205A8D") };

    public static readonly QuestionEntity QuestionToUpdate =
        DefaultQuestion with { Id = Guid.Parse("F6DB17FF-C68B-4831-8EA1-9D8B5408FC02") };

    public static readonly QuestionEntity QuestionInQuizToDelete = DefaultQuestion with
    {
        Id = Guid.Parse("51DC8540-1865-48D6-BA49-9696B9A431E0"), QuizTemplateId = QuizTemplateSeeds.QuizToDelete.Id
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuestionEntity>().HasData(
            DefaultQuestion with { Answers = Array.Empty<AnswerEntity>() },
            QuestionToDelete with { Answers = Array.Empty<AnswerEntity>() },
            QuestionToUpdate with { Answers = Array.Empty<AnswerEntity>() },
            QuestionInQuizToDelete with { Answers = Array.Empty<AnswerEntity>() }
        );
    }
}