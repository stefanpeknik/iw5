using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.DAL.Seeds;

public static class QuizSeeds
{
    public static readonly QuizEntity DefaultQuiz = new()
    {
        Id = Guid.Parse("EF2E391C-EA09-490B-9935-BBC7E7099A42"),
        Schedule = new DateTime(2023, 5, 5),
        Title = "Fun Trivia",
        Finished = true
    };

    public static readonly QuizEntity DefaultQuiz2 = new()
    {
        Id = Guid.Parse("ABA1F111-E4CA-47BB-803C-4D6F12EBF526"),
        Schedule = new DateTime(2023, 5, 6),
        Title = "Nejbystrejsi student FIT VUT",
        Finished = true
    };
    
    public static IEnumerable<QuizEntity> GetDefaultQuizes()
    {
        return new List<QuizEntity>()
        {
            DefaultQuiz with { Questions = new List<QuestionEntity>(), Scores = new List<ScoreEntity>() },
            DefaultQuiz2 with { Questions = new List<QuestionEntity>(), Scores = new List<ScoreEntity>() }
        };
    }
}