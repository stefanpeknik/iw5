using System.Net;
using System.Net.Http.Json;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Responses;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class QuizControllerTests : EndToEndTestsBase
{
    [Fact]
    public async Task GetAllQuizzes_Returns_At_Least_One_Quiz()
    {
        // Arrange
        var quizSeed = QuizSeeds.DefaultQuiz;
        var quizSeedModel = Mapper.Map<QuizListModel>(quizSeed);

        // Act
        var response = await Client.Value.GetAsync("/api/quizzes");
        response.EnsureSuccessStatusCode();
        var quizzes = await response.Content.ReadFromJsonAsync<ICollection<QuizListModel>>();

        // Assert
        Assert.NotNull(quizzes);
        Assert.NotEmpty(quizzes);
        Assert.Contains(quizzes, quiz => quiz.Id == quizSeedModel.Id);
    }

    [Fact]
    public async Task GetQuizById_Returns_Quiz_With_The_Same_Id()
    {
        // Arrange
        var quizSeed = QuizSeeds.DefaultQuiz;
        var quizSeedModel = Mapper.Map<QuizDetailModel>(quizSeed);

        // Act
        var response = await Client.Value.GetAsync($"/api/quizzes/{quizSeedModel.Id}");
        var quiz = await response.Content.ReadFromJsonAsync<QuizDetailModel>();

        // Assert
        Assert.NotNull(quiz);
        Assert.Equal(quizSeedModel.Id, quiz.Id);
    }

    [Fact]
    public async Task GetQuizById_Returns_NotFound_When_Quiz_Does_Not_Exist()
    {
        // Act
        var response = await Client.Value.GetAsync($"/api/quizzes/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateQuiz_Returns_Created_Quiz_Id()
    {
        // Arrange
        var quizSeed = QuizSeeds.DefaultQuiz;
        var quizSeedModel = Mapper.Map<QuizCreateUpdateModel>(quizSeed);

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/quizzes", quizSeedModel);
        var postId = await post.Content.ReadFromJsonAsync<IdModel>();
        var get = await Client.Value.GetAsync($"/api/quizzes/{postId!.Id}");
        var getId = (await get.Content.ReadFromJsonAsync<QuizDetailModel>())!.Id;

        // Assert
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        Assert.Equal(postId.Id, getId);
    }

    [Fact]
    public async Task CreateGarbage_Returns_BadRequest()
    {
        // Arrange
        var garbage = new { Garbage = "Garbage" };

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/quizzes", garbage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }

    [Fact]
    public async Task UpdateQuizById_Returns_Updated_Quiz_Id()
    {
        // Arrange
        var quizSeed = QuizSeeds.QuizToUpdate;
        var quizSeedModel = Mapper.Map<QuizDetailModel>(quizSeed);
        var quizSeedModelUpdated = Mapper.Map<QuizCreateUpdateModel>(quizSeed);
        quizSeedModelUpdated.Title = "Updated text";

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/quizzes/{quizSeedModel.Id}", quizSeedModelUpdated);
        var putId = await put.Content.ReadFromJsonAsync<IdModel>();
        var get = await Client.Value.GetAsync($"/api/quizzes/{putId!.Id}");
        var getId = (await get.Content.ReadFromJsonAsync<QuizDetailModel>())!.Id;

        // Assert
        Assert.Equal(HttpStatusCode.OK, put.StatusCode);
        Assert.Equal(putId.Id, getId);
    }

    [Fact]
    public async Task UpdateQuizById_Returns_NotFound_When_Quiz_Does_Not_Exist()
    {
        // Arrange
        var quizSeed = QuizSeeds.QuizToUpdate;
        var quizSeedModelUpdated = Mapper.Map<QuizDetailModel>(quizSeed);
        quizSeedModelUpdated.Title = "Updated text";
        var nonExistentId = Guid.NewGuid();

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/quizzes/{nonExistentId}", quizSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);
    }

    [Fact]
    public async Task UpdateQuizByIdWithGarbage_Returns_BadRequest()
    {
        // Arrange
        var quizSeed = QuizSeeds.QuizToUpdate;
        var quizSeedModelUpdated = Mapper.Map<QuizDetailModel>(quizSeed);
        var garbage = new { Garbage = "Garbage" };

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/quizzes/{quizSeedModelUpdated.Id}", garbage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }

    [Fact]
    public async Task DeleteQuizById_Returns_Ok()
    {
        // Arrange
        var quizSeed = QuizSeeds.QuizToDelete;
        var quizSeedModel = Mapper.Map<QuizDetailModel>(quizSeed);

        // Act
        var delete = await Client.Value.DeleteAsync($"/api/quizzes/{quizSeedModel.Id}");
        var get = await Client.Value.GetAsync($"/api/quizzes/{quizSeedModel.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, delete.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task DeleteQuizById_Returns_NotFound_When_Quiz_Does_Not_Exist()
    {
        // Arrange
        var nonexistentId = Guid.NewGuid();

        // Act
        var delete = await Client.Value.DeleteAsync($"/api/quizzes/{nonexistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);
    }
}