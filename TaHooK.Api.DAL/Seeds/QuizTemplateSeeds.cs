using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.DAL.Seeds;

public static class QuizTemplateSeeds
{
    public static readonly QuizTemplateEntity DefaultQuiz = new()
    {
        Id = Guid.Parse("EF2E391C-EA09-490B-9935-BBC7E7099A42"),
        Title = "Fun Trivia",
        CreatorId = UserSeeds.DefaultUser.Id,
        Creator = null!
    };

    public static readonly QuizTemplateEntity DefaultQuiz2 = new()
    {
        Id = Guid.Parse("ABA1F111-E4CA-47BB-803C-4D6F12EBF526"),
        Title = "Nejbystrejsi student FIT VUT",
        CreatorId = UserSeeds.DefaultUser.Id,
        Creator = null!
    };

    public static void Seed(this TaHooKDbContext dbContext)
    {
        if (!dbContext.QuizTemplates.Any())
        {
            var templates = new List<QuizTemplateEntity>()
            {
                DefaultQuiz with { Questions = new List<QuestionEntity>()},
                DefaultQuiz2 with { Questions = new List<QuestionEntity>()}
            };

            dbContext.QuizTemplates.AddRange(templates);
        }
    }
}