using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.DAL.Seeds;

public static class AnswerSeeds
{
    public static readonly AnswerEntity DefaultAnswer = new()
    {
        Id = Guid.Parse("DE80D56E-DDCA-4500-9E7C-7E72C3838E50"),
        IsCorrect = true,
        Picture = new Uri(
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Bananas_white_background_DS.jpg/220px-Bananas_white_background_DS.jpg"),
        Question = null!,
        QuestionId = QuestionSeeds.DefaultQuestion.Id,
        Text = "Banana",
        Type = default
    };

    public static readonly AnswerEntity DefaultAnswer2 = new()
    {
        Id = Guid.Parse("F7C63A36-BA31-4DBE-BB57-DAD497BC876A"),
        IsCorrect = true,
        Picture = new Uri(
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Autumn_Red_peaches.jpg/2560px-Autumn_Red_peaches.jpg"),
        Question = null!,
        QuestionId = QuestionSeeds.DefaultQuestion2.Id,
        Text = "Peach",
        Type = default
    };

    public static IEnumerable<AnswerEntity> GetDefaultAnswers()
    {
        return new List<AnswerEntity>()
        {
            DefaultAnswer,
            DefaultAnswer2
        };
    }
}