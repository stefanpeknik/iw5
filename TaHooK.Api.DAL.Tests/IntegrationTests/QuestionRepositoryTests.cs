using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests.IntegrationTests;

public class QuestionRepositoryTests: DALTestsBase
{
    public QuestionRepositoryTests(ITestOutputHelper output) : base(output) { }
    
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
    public async Task Exists_Question_True()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuestionEntity>();
        
        // Act
        var result = await repository.ExistsAsync(QuestionSeeds.DefaultQuestion.Id);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task Exists_Question_False()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuestionEntity>();
        var question = QuestionSeeds.DefaultQuestion with { Id = Guid.NewGuid() };
        
        
        // Act
        var result = await repository.ExistsAsync(question.Id);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task InsertNew_Question()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuestionEntity>();
        var newQuestion = QuestionSeeds.DefaultQuestion with { Id = Guid.NewGuid() };
        
        // Act
        var insertedEntity = await repository.InsertAsync(newQuestion);
        await UnitOfWork.CommitAsync();
        
        // Assert
        var retrieved = await DbContextInstance.Questions.FindAsync(insertedEntity.Id);
        Assert.Equal(newQuestion, retrieved);
    }

    [Fact]
    public async Task Update_Question()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuestionEntity>();
        var updated = QuestionSeeds.QuestionToUpdate with { Text = "Updated text" };
        
        // Act
        await repository.UpdateAsync(updated);
        await UnitOfWork.CommitAsync();
        
        // Assert
        var contains = await DbContextInstance.Questions.ContainsAsync(updated);
        Assert.True(contains);
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
        var contains = await DbContextInstance.Questions.ContainsAsync(QuestionSeeds.QuestionToDelete);
        Assert.False(contains);
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
        var contains = await DbContextInstance.Answers.ContainsAsync(AnswerSeeds.AnswerUnderQuestionToDelete);
        Assert.False(contains);
    }
}