using DeepEqual.Syntax;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Common.Entities;
using TaHooK.Api.DAL.Common.Entities.Interfaces;
using TaHooK.Api.DAL.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class RepositoriesTests : DALTestsBase
    {

        public RepositoriesTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void GetAll_Questions()
        {
            // Arrange
            var repository = new RepositoryBase<QuestionEntity>(DbContextInstance);

            // Act
            var result = repository.GetAll();

            // Assert
            Assert.True(result.Contains(QuestionSeeds.DefaultQuestion));
            Assert.True(result.Contains(QuestionSeeds.QuestionToDelete));
        }

        [Fact]
        public async Task GetById_User()
        {
            // Arrange
            var repository = new RepositoryBase<UserEntity>(DbContextInstance);

            // Act
            var result = await repository.GetByIdAsync(UserSeeds.DefaultUser.Id);

            // Assert
            Assert.True(result.IsDeepEqual(UserSeeds.DefaultUser));
        }

        [Fact]
        public async Task InsertNew_Answer()
        {
            // Arrange
            var repository = new RepositoryBase<AnswerEntity>(DbContextInstance);
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

            // Assert
            var retrieved = await repository.GetByIdAsync(insertedId);
            Assert.NotNull(retrieved);
            Assert.True(retrieved.IsDeepEqual(entity));
        }

        [Fact]
        public async Task Update_Score()
        {
            // Arrange
            var repository = new RepositoryBase<ScoreEntity>(DbContextInstance);
            var updated = ScoreSeeds.DefaultScore with { Score = 123 };

            // Act
            await repository.UpdateAsync(updated);

            // Assert
            var retrieved = await repository.GetByIdAsync(ScoreSeeds.DefaultScore.Id);
            Assert.NotNull(retrieved);
            Assert.Equal(123, retrieved.Score);
        }

        [Fact]
        public async Task Delete_Question()
        {
            // Arrange
            var repository = new RepositoryBase<QuestionEntity>(DbContextInstance);

            // Act
            await repository.DeleteAsync(QuestionSeeds.QuestionToDelete.Id);
            await DbContextInstance.SaveChangesAsync();

            // Assert
            Assert.Null(await repository.GetByIdAsync(QuestionSeeds.QuestionToDelete.Id));
        }
    }
}