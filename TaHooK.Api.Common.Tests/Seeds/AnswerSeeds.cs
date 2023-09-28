using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaHooK.Api.DAL.Common.Entities;

namespace TaHooK.Api.Common.Tests.Seeds
{
    public static class AnswerSeeds
    {

        public static readonly AnswerEntity DefaultAnswer = new()
        {
            Id = Guid.Parse("DE80D56E-DDCA-4500-9E7C-7E72C3838E50"),
            IsCorrect = false,
            Picture = new Uri(
                "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Bananas_white_background_DS.jpg/220px-Bananas_white_background_DS.jpg"),
            Question = QuestionSeeds.DefaultQuestion,
            QuestionId = Guid.Parse("574E8D70-47A1-4D76-B971-CAFB314540FD"),
            Text = "Banana",
            Type = default
        };

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerEntity>().HasData(
                DefaultAnswer
            );
        }
    }
}
