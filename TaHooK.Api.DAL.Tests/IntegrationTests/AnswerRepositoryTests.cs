using DeepEqual.Syntax;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests.IntegrationTests;

public class AnswerRepositoryTests: DALTestsBase
{
    public AnswerRepositoryTests(ITestOutputHelper output) : base(output) { }
    
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
        var insertedEntity = await repository.InsertAsync(entity);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.Answers.FindAsync(insertedEntity.Id);
        Assert.True(retrieved.IsDeepEqual(entity));
    }
}