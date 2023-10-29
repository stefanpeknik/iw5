using System.Net;
using System.Net.Http.Json;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Common.Models.Responses;
using TaHooK.Common.Models.Score;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class ScoreControllerTests : EndToEndTestsBase
{
    [Fact]
    public async Task GetAllScores_Returns_At_Least_One_Score()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.DefaultScore;
        var scoreSeedModel = Mapper.Map<ScoreListModel>(scoreSeed);

        // Act
        var response = await Client.Value.GetAsync("/api/scores");
        response.EnsureSuccessStatusCode();
        var scores = await response.Content.ReadFromJsonAsync<ICollection<ScoreListModel>>();

        // Assert
        Assert.NotNull(scores);
        Assert.NotEmpty(scores);
        Assert.Contains(scores, score => score.Id == scoreSeedModel.Id);
    }

    [Fact]
    public async Task GetScoreById_Returns_Score_With_The_Same_Id()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.DefaultScore;
        var scoreSeedModel = Mapper.Map<ScoreDetailModel>(scoreSeed);

        // Act
        var response = await Client.Value.GetAsync($"/api/scores/{scoreSeedModel.Id}");
        var score = await response.Content.ReadFromJsonAsync<ScoreDetailModel>();

        // Assert
        Assert.NotNull(score);
        Assert.Equal(scoreSeedModel.Id, score.Id);
    }

    [Fact]
    public async Task GetScoreById_Returns_NotFound_When_Score_Does_Not_Exist()
    {
        // Act
        var response = await Client.Value.GetAsync($"/api/scores/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateScore_Returns_Created_Score_Id()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.DefaultScore;
        var scoreSeedModel = Mapper.Map<ScoreCreateUpdateModel>(scoreSeed);

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/scores", scoreSeedModel);
        var postId = await post.Content.ReadFromJsonAsync<IdModel>();
        var get = await Client.Value.GetAsync($"/api/scores/{postId?.Id}");
        var getId = (await get.Content.ReadFromJsonAsync<ScoreDetailModel>())!.Id;

        // Assert
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        Assert.Equal(postId?.Id, getId);
    }

    [Fact]
    public async Task CreateScoreForNonExistentUser_Returns_BadRequest()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.DefaultScore;
        var scoreSeedModel = Mapper.Map<ScoreDetailModel>(scoreSeed);
        scoreSeedModel.UserId = Guid.NewGuid();

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/scores", scoreSeedModel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }

    [Fact]
    public async Task CreateScoreForNonExistentQuiz_Returns_BadRequest()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.DefaultScore;
        var scoreSeedModel = Mapper.Map<ScoreDetailModel>(scoreSeed);
        scoreSeedModel.QuizId = Guid.NewGuid();

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/scores", scoreSeedModel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }

    [Fact]
    public async Task CreateGarbage_Returns_BadRequest()
    {
        // Arrange
        var garbage = new { Garbage = "Garbage" };

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/scores", garbage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }

    [Fact]
    public async Task UpdateScoreById_Returns_Updated_Score_Id()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.ScoreToUpdate;
        var scoreSeedModel = Mapper.Map<ScoreDetailModel>(scoreSeed);
        var scoreSeedModelUpdated = Mapper.Map<ScoreCreateUpdateModel>(scoreSeed);
        scoreSeedModelUpdated.Score = 3;

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/scores/{scoreSeedModel.Id}", scoreSeedModelUpdated);
        var putId = await put.Content.ReadFromJsonAsync<IdModel>();
        var get = await Client.Value.GetAsync($"/api/scores/{putId?.Id}");
        var getId = (await get.Content.ReadFromJsonAsync<ScoreDetailModel>())!.Id;

        // Assert
        Assert.Equal(HttpStatusCode.OK, put.StatusCode);
        Assert.Equal(putId?.Id, getId);
    }

    [Fact]
    public async Task UpdateScoreByIdForNonExistentUser_Returns_BadRequest()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.ScoreToUpdate;
        var scoreSeedModelUpdated = Mapper.Map<ScoreDetailModel>(scoreSeed);
        scoreSeedModelUpdated.UserId = Guid.NewGuid();

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/scores/{scoreSeedModelUpdated.Id}", scoreSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }

    [Fact]
    public async Task UpdateScoreByIdForNonExistentQuiz_Returns_BadRequest()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.ScoreToUpdate;
        var scoreSeedModelUpdated = Mapper.Map<ScoreDetailModel>(scoreSeed);
        scoreSeedModelUpdated.QuizId = Guid.NewGuid();

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/scores/{scoreSeedModelUpdated.Id}", scoreSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }

    [Fact]
    public async Task UpdateScoreById_Returns_NotFound_When_Score_Does_Not_Exist()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.ScoreToUpdate;
        var scoreSeedModelUpdated = Mapper.Map<ScoreDetailModel>(scoreSeed);
        scoreSeedModelUpdated.Score = 3;
        var nonExistentId = Guid.NewGuid();

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/scores/{nonExistentId}", scoreSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);
    }

    [Fact]
    public async Task UpdateScoreByIdWithGarbage_Returns_BadRequest()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.ScoreToUpdate;
        var scoreSeedModelUpdated = Mapper.Map<ScoreDetailModel>(scoreSeed);
        var garbage = new { Garbage = "Garbage" };

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/scores/{scoreSeedModelUpdated.Id}", garbage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }

    [Fact]
    public async Task DeleteScoreById_Returns_Ok()
    {
        // Arrange
        var scoreSeed = ScoreSeeds.ScoreToDelete;
        var scoreSeedModel = Mapper.Map<ScoreDetailModel>(scoreSeed);

        // Act
        var delete = await Client.Value.DeleteAsync($"/api/scores/{scoreSeedModel.Id}");
        var get = await Client.Value.GetAsync($"/api/scores/{scoreSeedModel.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, delete.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task DeleteScoreById_Returns_NotFound_When_Score_Does_Not_Exist()
    {
        // Arrange
        var nonexistentId = Guid.NewGuid();

        // Act
        var delete = await Client.Value.DeleteAsync($"/api/scores/{nonexistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);
    }
}