using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests.IntegrationTests;

public class ScoreRepositoryTests : DALTestsBase
{
    public ScoreRepositoryTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetAll_Scores()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<ScoreEntity>();

        // Act
        var result = repository.Get();

        // Assert
        Assert.True(result.Contains(ScoreSeeds.DefaultScore));
        Assert.True(result.Contains(ScoreSeeds.ScoreToDelete));
    }

    [Fact]
    public async Task Exists_Score_True()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<ScoreEntity>();

        // Act
        var result = await repository.ExistsAsync(ScoreSeeds.DefaultScore.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exists_Score_False()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<ScoreEntity>();
        var score = ScoreSeeds.DefaultScore with { Id = Guid.NewGuid() };


        // Act
        var result = await repository.ExistsAsync(score.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task InsertNew_Score()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<ScoreEntity>();
        var newScore = ScoreSeeds.DefaultScore with { Id = Guid.NewGuid() };

        // Act
        await repository.InsertAsync(newScore);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.Scores.FindAsync(newScore.Id);
        Assert.Equal(newScore, retrieved);
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
        var contains = await DbContextInstance.Scores.ContainsAsync(updated);
        Assert.True(contains);
    }

    [Fact]
    public async Task Delete_Score()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<ScoreEntity>();
        var score = ScoreSeeds.ScoreToDelete;

        // Act
        await repository.DeleteAsync(score.Id);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.Scores.FindAsync(score.Id);
        Assert.Null(retrieved);
    }
}