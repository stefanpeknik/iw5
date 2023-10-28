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
    public void GetAll_Answers()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<AnswerEntity>();
        
        // Act
        var result = repository.Get();
        
        // Assert
        Assert.True(result.Contains(AnswerSeeds.DefaultAnswer));
        Assert.True(result.Contains(AnswerSeeds.AnswerUnderQuestionToDelete));
    }
    
    [Fact]
    public async Task Exists_Answer_True()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<AnswerEntity>();
        
        // Act
        var result = await repository.ExistsAsync(AnswerSeeds.DefaultAnswer.Id);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task Exists_Answer_False()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<AnswerEntity>();
        var answer = AnswerSeeds.DefaultAnswer with { Id = Guid.NewGuid() };
        
        
        // Act
        var result = await repository.ExistsAsync(answer.Id);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task InsertNew_Answer()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<AnswerEntity>();
        var newEntity = AnswerSeeds.DefaultAnswer with { Id = Guid.NewGuid() }; 

        // Act
        var insertedEntity = await repository.InsertAsync(newEntity);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.Answers.FindAsync(insertedEntity.Id);
        Assert.Equal(newEntity, retrieved);
    }
    
    [Fact]
    public async Task Update_Answer()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<AnswerEntity>();
        var updated = AnswerSeeds.AnswerToUpdate with { Text = "Updated" };

        // Act
        await repository.UpdateAsync(updated);
        await UnitOfWork.CommitAsync();

        // Assert
        var contains = await DbContextInstance.Answers.ContainsAsync(updated);
        Assert.True(contains);
    }
    
    [Fact]
    public async Task Delete_Answer()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<AnswerEntity>();
        var answerToDelete = AnswerSeeds.AnswerToDelete;

        // Act
        await repository.DeleteAsync(answerToDelete.Id);
        await UnitOfWork.CommitAsync();

        // Assert
        var contains = await DbContextInstance.Answers.ContainsAsync(answerToDelete);
        Assert.False(contains);
    }
}