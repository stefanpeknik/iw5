using AutoMapper;
using DeepEqual.Syntax;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.Entities.Interfaces;
using TaHooK.Api.DAL.Repositories;
using TaHooK.Api.DAL.UnitOfWork;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class RepositoriesTests : DALTestsBase
    {

        public RepositoriesTests(ITestOutputHelper output, IMapper mapper) : base(output, mapper) { }

        [Fact]
        public void GetAll_Questions()
        {
            // Arrange
            var repository = UnitOfWork.GetRepository<QuestionEntity>();

            // Act
            var result = repository.Get();

            // Assert
            Assert.True(result.Contains(QuestionSeeds.DefaultQuestion));
            Assert.True(result.Contains(QuestionSeeds.QuestionToDelete));
        }

        [Fact]
        public async Task InsertNew_Answer()
        {
            // Arrange
            var repository = UnitOfWork.GetRepository<AnswerEntity>();
            var entity = new AnswerEntity()
            {
                Id = Guid.NewGuid(),
                IsCorrect = true,
                Picture = null,
                Question = null!,
                QuestionId = QuestionSeeds.DefaultQuestion.Id,
                Text = "Newly added question",
                Type = default
            };

            // Act
            var insertedId = await repository.InsertAsync(entity);
            await UnitOfWork.CommitAsync();

            // Assert
            var retrieved = await repository.Get().Where(i => i.Id == insertedId).SingleAsync();
            Assert.True(retrieved.IsDeepEqual(entity));
        }

        [Fact]
        public async Task Update_Score()
        {
            // Arrange
            var repository = UnitOfWork.GetRepository<ScoreEntity>();
            var updated = ScoreSeeds.DefaultScore with { Score = 123 };

            // Act
            await repository.UpdateAsync(updated);
            await UnitOfWork.CommitAsync();

            // Assert
            var retrieved = await repository.Get()
                .Where(i => i.Id == ScoreSeeds.DefaultScore.Id).SingleAsync();
            Assert.Equal(123, retrieved.Score);
        }

        [Fact]
        public async Task Delete_Question()
        {
            // Arrange
            var repository = UnitOfWork.GetRepository<QuestionEntity>();


            // Act
            await repository.DeleteAsync(QuestionSeeds.QuestionToDelete.Id);
            await UnitOfWork.CommitAsync();

            // Assert
            Assert.False(await repository.Get().ContainsAsync(QuestionSeeds.QuestionToDelete));
        }

        [Fact]
        public async Task Delete_Question_Cascades()
        {
            // Arrange
            var repository = UnitOfWork.GetRepository<QuestionEntity>();

            // Act
            await repository.DeleteAsync(QuestionSeeds.QuestionToDelete.Id);
            await UnitOfWork.CommitAsync();
            // Should also cascade delete the answer

            // Assert
            var answerRepository = UnitOfWork.GetRepository<AnswerEntity>();
            Assert.False(await answerRepository.Get().ContainsAsync(AnswerSeeds.AnswerUnderQuestionToDelete));
        }
    }
}