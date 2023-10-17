using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests.IntegrationTests;

public class ScoreRepositoryTests: DALTestsBase
{
    public ScoreRepositoryTests(ITestOutputHelper output) : base(output) { }
    
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
        var retrieved = await DbContextInstance.Scores.FindAsync(updated.Id);
        Assert.Equal(123, retrieved?.Score);
    }
}