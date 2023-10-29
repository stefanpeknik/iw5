using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.Common.Tests.Seeds;

public static class AnswerSeeds
{
    public static readonly AnswerEntity DefaultAnswer = new()
    {
        Id = Guid.Parse("DE80D56E-DDCA-4500-9E7C-7E72C3838E50"),
        IsCorrect = true,
        Picture = new Uri(
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Bananas_white_background_DS.jpg/220px-Bananas_white_background_DS.jpg"),
        Question = null!,
        QuestionId = Guid.Parse("574E8D70-47A1-4D76-B971-CAFB314540FD"),
        Text = "Banana",
        Type = default
    };

    public static readonly AnswerEntity AnswerToDelete =
        DefaultAnswer with { Id = Guid.Parse("4674413D-5F62-4600-B508-4F75EB959E38") };

    public static readonly AnswerEntity AnswerToUpdate =
        DefaultAnswer with { Id = Guid.Parse("C455EB61-CC50-4AF1-8A55-C674B146D53E") };

    public static readonly AnswerEntity AnswerUnderQuestionToDelete =
        DefaultAnswer with
        {
            Id = Guid.Parse("2A14549B-3547-46A1-B4AF-8E398F0B55B0"), QuestionId = QuestionSeeds.QuestionToDelete.Id
        };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnswerEntity>().HasData(
            DefaultAnswer,
            AnswerUnderQuestionToDelete,
            AnswerToDelete,
            AnswerToUpdate
        );
    }
}