using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class DbContextTests : DALTestsBase
    {

        public DbContextTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GetById_Quiz_Including_Questions_And_Answers()
        {
            //Act
            var quiz = await DbContextInstance.Quizes
                .Where(i => i.Id == QuizSeeds.DefaultQuiz.Id)
                .Include(i => i.Questions)
                .SingleAsync();

            var question = await DbContextInstance.Questions
                .Where(i => i.Id == QuestionSeeds.DefaultQuestion.Id)
                .Include(i => i.Answers)
                .SingleAsync();

            //Assert
            Assert.NotNull(quiz);
            Assert.Equal(QuizSeeds.DefaultQuiz.Id, quiz.Id);

            Assert.NotEmpty(quiz.Questions);
            Assert.Contains(quiz.Questions, i => i.Id == QuestionSeeds.DefaultQuestion.Id);

            Assert.NotNull(question);
            Assert.Equal(QuestionSeeds.DefaultQuestion.Id, question.Id);

            Assert.NotEmpty(question.Answers);
            Assert.Contains(question.Answers, i => i.Id == AnswerSeeds.DefaultAnswer.Id);
        }

        [Fact]
        public async Task Add_New_Answer_To_Existing_Question()
        {
            // Arrange
            var question = await DbContextInstance.Questions
                .Where(i => i.Id == QuestionSeeds.DefaultQuestion.Id)
                .Include(i => i.Answers)
                .SingleAsync();

            Assert.NotNull(question);

            var beforeCount = question.Answers.Count;

            // Act
            await DbContextInstance.Answers.AddAsync(new AnswerEntity()
            {
                Id = new Guid(),
                IsCorrect = true,
                Picture = null,
                Question = null!,
                QuestionId = question.Id,
                Text = "Newly added answer",
                Type = default
            });

            await DbContextInstance.SaveChangesAsync();

            question = await DbContextInstance.Questions
                .Where(i => i.Id == QuestionSeeds.DefaultQuestion.Id)
                .Include(i => i.Answers)
                .SingleAsync();

            // Assert
            Assert.NotNull(question);
            Assert.Contains(question.Answers, entity => entity.Text == "Newly added answer");
        }

        [Fact]
        public async Task Delete_Question()
        {
            // Act
            DbContextInstance.Questions.Remove(QuestionSeeds.QuestionToDelete);

            await DbContextInstance.SaveChangesAsync();

            var result = await DbContextInstance.Questions
                .Where(i => i.Id == QuestionSeeds.QuestionToDelete.Id)
                .SingleOrDefaultAsync();

            // Assert
            Assert.Null(result);
        }
    }
}