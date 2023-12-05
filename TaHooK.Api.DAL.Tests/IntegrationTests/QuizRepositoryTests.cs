using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests.IntegrationTests;

public class QuizRepositoryTests : DALTestsBase
{
    public QuizRepositoryTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetAll_Quizzes()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizTemplateEntity>();

        // Act
        var result = repository.Get();

        // Assert
        Assert.True(result.Contains(QuizTemplateSeeds.DefaultQuiz));
        Assert.True(result.Contains(QuizTemplateSeeds.QuizToDelete));
    }

    [Fact]
    public async Task Exists_Quiz_True()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizTemplateEntity>();

        // Act
        var result = await repository.ExistsAsync(QuizTemplateSeeds.DefaultQuiz.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exists_Quiz_False()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizTemplateEntity>();
        var quiz = QuizTemplateSeeds.DefaultQuiz with { Id = Guid.NewGuid() };


        // Act
        var result = await repository.ExistsAsync(quiz.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task InsertNew_Quiz()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizTemplateEntity>();
        var newQuiz = QuizTemplateSeeds.DefaultQuiz with { Id = Guid.NewGuid() };

        // Act
        await repository.InsertAsync(newQuiz);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.QuizTemplates.FindAsync(newQuiz.Id);
        Assert.Equal(newQuiz, retrieved);
    }

    [Fact]
    public async Task Update_Quiz()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizTemplateEntity>();
        var updated = QuizTemplateSeeds.QuizToUpdate with { Title = "Updated title" };

        // Act
        await repository.UpdateAsync(updated);
        await UnitOfWork.CommitAsync();

        // Assert
        var contains = await DbContextInstance.QuizTemplates.ContainsAsync(updated);
        Assert.True(contains);
    }

    [Fact]
    public async Task Delete_Quiz()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizTemplateEntity>();
        var quiz = QuizTemplateSeeds.QuizToDelete;

        // Act
        await repository.DeleteAsync(quiz.Id);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.QuizTemplates.FindAsync(quiz.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task Delete_Quiz_With_Questions_Cascades()
    {
        // Arrange
        var repository = UnitOfWork.GetRepository<QuizTemplateEntity>();

        // Act
        await repository.DeleteAsync(QuizTemplateSeeds.QuizToDelete.Id);
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
        await repository.DeleteAsync(QuizSeeds.DefaultQuiz.Id);
        await UnitOfWork.CommitAsync();

        // Assert
        var retrieved = await DbContextInstance.Scores.FindAsync(ScoreSeeds.ScoreInQuizToDelete.Id);
        Assert.Null(retrieved);
    }
}