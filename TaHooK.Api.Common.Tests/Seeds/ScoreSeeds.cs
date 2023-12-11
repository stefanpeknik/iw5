using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.Common.Tests.Seeds;

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

    public static readonly ScoreEntity ScoreToDelete =
        DefaultScore with { Id = Guid.Parse("5BA3EC1D-3602-4E33-A213-61A11684CF22") };

    public static readonly ScoreEntity ScoreToUpdate =
        DefaultScore with { Id = Guid.Parse("4C603830-0985-4190-925C-9050A5F4F406") };

    public static readonly ScoreEntity ScoreInQuizToDelete = DefaultScore with
    {
        Id = Guid.Parse("E0D0D40E-298F-4958-885A-1EFCF89DA83F"), QuizId = QuizSeeds.DefaultQuiz.Id
    };

    public static readonly ScoreEntity ScoreWithUserToDelete = DefaultScore with
    {
        Id = Guid.Parse("E4C722B6-E9B9-4207-BFC4-580945F83E61"), UserId = UserSeeds.UserToDelete.Id
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScoreEntity>().HasData(
            DefaultScore,
            ScoreToDelete,
            ScoreToUpdate,
            ScoreInQuizToDelete,
            ScoreWithUserToDelete
        );
    }
}