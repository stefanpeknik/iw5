using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.DAL.Seeds;

public static class QuizSeeds
{
    public static readonly QuizEntity DefaultQuiz = new()
    {
        Id = Guid.Parse("CFF79175-AB62-4DF0-BB77-1CD6F940CEE1"),
        Title = "Fun Trivia",
        Finished = true,
        StartedAt = DateTime.Now,
        TemplateId = QuizTemplateSeeds.DefaultQuiz.Id,
        Template = null!
    };

    public static readonly QuizEntity DefaultQuiz2 = new()
    {
        Id = Guid.Parse("DF6351D3-1093-4FD5-99CB-C050B8E0E531"),
        Title = "Nejbystrejsi student FIT VUT",
        Finished = false,
        StartedAt = DateTime.Now,
        TemplateId = QuizTemplateSeeds.DefaultQuiz2.Id,
        Template = null!
    };

    public static void Seed(this TaHooKDbContext dbContext)
    {
        if (!dbContext.Quizes.Any())
        {
            var templates = new List<QuizEntity>()
            {
                DefaultQuiz with { Scores = new List<ScoreEntity>()},
                DefaultQuiz2 with { Scores = new List<ScoreEntity>()}
            };

            dbContext.Quizes.AddRange(templates);
        }
    }
}