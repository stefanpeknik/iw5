using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests.IntegrationTests;

public class QuizRepositoryTests: DALTestsBase
{
    public QuizRepositoryTests(ITestOutputHelper output) : base(output) { }
    
    [Fact]
    public void GetAll_Quizzes()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizEntity>();

        // Act
        var result = repository.Get();

        // Assert
        Assert.True(result.Contains(QuizSeeds.DefaultQuiz));
        Assert.True(result.Contains(QuizSeeds.QuizToDelete));
    }
    
    [Fact]
    public async Task Exists_Quiz_True()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizEntity>();
        
        // Act
        var result = await repository.ExistsAsync(QuizSeeds.DefaultQuiz.Id);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task Exists_Quiz_False()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizEntity>();
        var quiz = QuizSeeds.DefaultQuiz with { Id = Guid.NewGuid() };
        
        
        // Act
        var result = await repository.ExistsAsync(quiz.Id);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task InsertNew_Quiz()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizEntity>();
        var newQuiz = QuizSeeds.DefaultQuiz with { Id = Guid.NewGuid() };
        
        // Act
        await repository.InsertAsync(newQuiz);
        await UnitOfWork.CommitAsync();
        
        // Assert
        var retrieved = await DbContextInstance.Quizes.FindAsync(newQuiz.Id);
        Assert.Equal(newQuiz, retrieved);
    }
    
    [Fact]
    public async Task Update_Quiz()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizEntity>();
        var updated = QuizSeeds.QuizToUpdate with { Title = "Updated title" };

        // Act
        await repository.UpdateAsync(updated);
        await UnitOfWork.CommitAsync();

        // Assert
        var contains = await DbContextInstance.Quizes.ContainsAsync(updated);
        Assert.True(contains);
    }
    
    [Fact]
    public async Task Delete_Quiz()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizEntity>();
        var quiz = QuizSeeds.QuizToDelete;
        
        // Act
        await repository.DeleteAsync(quiz.Id);
        await UnitOfWork.CommitAsync();
        
        // Assert
        var retrieved = await DbContextInstance.Quizes.FindAsync(quiz.Id);
        Assert.Null(retrieved);
    }
    
    [Fact]
    public async Task Delete_Quiz_With_Questions_Cascades()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizEntity>();
        
        // Act
        await repository.DeleteAsync(QuizSeeds.QuizToDelete.Id);
        await UnitOfWork.CommitAsync();
        
        // Assert
        var retrieved = await DbContextInstance.Questions.FindAsync(QuestionSeeds.QuestionInQuizToDelete.Id);
        Assert.Null(retrieved);
    }
    
    [Fact]
    public async Task Delete_Quiz_With_Scores_Cascades()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizEntity>();
        
        // Act
        await repository.DeleteAsync(QuizSeeds.QuizToDelete.Id);
        await UnitOfWork.CommitAsync();
        
        // Assert
        var retrieved = await DbContextInstance.Scores.FindAsync(ScoreSeeds.ScoreInQuizToDelete.Id);
        Assert.Null(retrieved);
    }
}