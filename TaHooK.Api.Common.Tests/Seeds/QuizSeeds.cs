using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.Common.Tests.Seeds;

public static class QuizSeeds
{
    public static readonly QuizEntity DefaultQuiz = new()
    {
        Id = Guid.Parse("60754673-0373-4835-8FEA-AF865AFE34B0"),
        Title = "Fun Trivia",
        Finished = true,
        StartedAt = DateTime.Now,
        TemplateId = QuizTemplateSeeds.DefaultQuiz.Id,
        Template = null!,
        CreatorId = UserSeeds.DefaultUser.Id,
        Creator = null!
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuizEntity>().HasData(
            DefaultQuiz with { Scores = Array.Empty<ScoreEntity>()}
        );
    }
}