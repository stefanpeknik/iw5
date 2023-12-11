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
        IsCorrect = false,
        Picture = new Uri(
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Autumn_Red_peaches.jpg/2560px-Autumn_Red_peaches.jpg"),
        Question = null!,
        QuestionId = QuestionSeeds.DefaultQuestion2.Id,
        Text = "Peach",
        Type = default
    };

    public static readonly AnswerEntity DefaultAnswer2_1 = new()
    {
        Id = Guid.Parse("FA3738B0-06C5-447D-AB81-1FD9B45ED1F2"),
        IsCorrect = true,
        Picture = new Uri(
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Bananas_white_background_DS.jpg/220px-Bananas_white_background_DS.jpg"),
        Question = null!,
        QuestionId = QuestionSeeds.DefaultQuestion2.Id,
        Text = "Banana",
        Type = default
    };

    public static readonly AnswerEntity DefaultAnswer2_2 = new()
    {
        Id = Guid.Parse("12EBD1AF-864E-4383-BF4C-371182EEC308"),
        IsCorrect = false,
        Picture = new Uri(
            "https://eshop.sklizeno.cz/data/images/product/498x498/400/400734_1.jpg?-62167222664"),
        Question = null!,
        QuestionId = QuestionSeeds.DefaultQuestion2.Id,
        Text = "Ananas",
        Type = default
    };

    public static readonly AnswerEntity DefaultAnswer3 = new()
    {
        Id = Guid.Parse("FF5BE80D-8932-4D23-8547-5A0670C46486"),
        IsCorrect = false,
        Picture = new Uri(
            "https://eshop.sklizeno.cz/data/images/product/498x498/400/400734_1.jpg?-62167222664"),
        Question = null!,
        QuestionId = QuestionSeeds.DefaultQuestion3.Id,
        Text = "Ananas",
        Type = default
    };

    public static readonly AnswerEntity DefaultAnswer3_1 = new()
    {
        Id = Guid.Parse("8839003D-B37C-41DE-B9BC-F455820B43BA"),
        IsCorrect = true,
        Picture = new Uri(
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Autumn_Red_peaches.jpg/2560px-Autumn_Red_peaches.jpg"),
        Question = null!,
        QuestionId = QuestionSeeds.DefaultQuestion3.Id,
        Text = "Peach",
        Type = default
    };

    public static readonly AnswerEntity DefaultAnswer3_2 = new()
    {
        Id = Guid.Parse("F277E19F-1A88-426D-8AE2-C558411F8005"),
        IsCorrect = false,
        Picture = new Uri(
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Bananas_white_background_DS.jpg/220px-Bananas_white_background_DS.jpg"),
        Question = null!,
        QuestionId = QuestionSeeds.DefaultQuestion3.Id,
        Text = "Banana",
        Type = default
    };

    public static void Seed(this TaHooKDbContext dbContext)
    {
        
        if (!dbContext.Answers.Any())
        {
            var answers = new List<AnswerEntity>()
            {
                DefaultAnswer,
                DefaultAnswer2,
                DefaultAnswer2_1,
                DefaultAnswer2_2,
                DefaultAnswer3,
                DefaultAnswer3_1,
                DefaultAnswer3_2
            };

            dbContext.Answers.AddRange(answers);
        }
    }
}