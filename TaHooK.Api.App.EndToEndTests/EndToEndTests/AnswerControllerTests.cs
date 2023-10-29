using System.Net;
using System.Net.Http.Json;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Responses;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class AnswerControllerTests : EndToEndTestsBase
{
    [Fact]
    public async Task GetAllAnswers_Returns_At_Least_One_Answer()
    {
        // Arrange
        var answerSeed = AnswerSeeds.DefaultAnswer;
        var answerSeedModel = Mapper.Map<AnswerListModel>(answerSeed);

        // Act
        var response = await Client.Value.GetAsync("/api/answers");
        response.EnsureSuccessStatusCode();
        var answers = await response.Content.ReadFromJsonAsync<ICollection<AnswerListModel>>();

        // Assert
        Assert.NotNull(answers);
        Assert.NotEmpty(answers);
        Assert.Contains(answers, answer => answer.Id == answerSeedModel.Id);
    }

    [Fact]
    public async Task GetAnswerById_Returns_Answer_With_The_Same_Id()
    {
        // Arrange
        var answerSeed = AnswerSeeds.DefaultAnswer;
        var answerSeedModel = Mapper.Map<AnswerDetailModel>(answerSeed);

        // Act
        var response = await Client.Value.GetAsync($"/api/answers/{answerSeedModel.Id}");
        var answer = await response.Content.ReadFromJsonAsync<AnswerDetailModel>();

        // Assert
        Assert.NotNull(answer);
        Assert.Equal(answerSeedModel.Id, answer.Id);
    }

    [Fact]
    public async Task GetAnswerById_Returns_NotFound_When_Answer_Does_Not_Exist()
    {
        // Act
        var response = await Client.Value.GetAsync($"/api/answers/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateAnswer_Returns_Created_Answer_Id()
    {
        // Arrange
        var answerSeed = AnswerSeeds.DefaultAnswer;
        var answerSeedModel = Mapper.Map<AnswerDetailModel>(answerSeed);

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/answers", answerSeedModel);
        var postId = await post.Content.ReadFromJsonAsync<IdModel>();
        var get = await Client.Value.GetAsync($"/api/answers/{postId!.Id}");
        var getId = (await get.Content.ReadFromJsonAsync<AnswerDetailModel>())!.Id;

        // Assert
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        Assert.Equal(postId.Id, getId);
    }

    [Fact]
    public async Task CreateAnswerForNonExistentQuestion_Returns_BadRequest()
    {
        // Arrange
        var answerSeed = AnswerSeeds.DefaultAnswer;
        var answerSeedModel = Mapper.Map<AnswerDetailModel>(answerSeed);
        answerSeedModel.QuestionId = Guid.NewGuid();

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/answers", answerSeedModel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }

    [Fact]
    public async Task CreateGarbage_Returns_BadRequest()
    {
        // Arrange
        var garbage = new { Garbage = "Garbage" };

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/answers", garbage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }

    [Fact]
    public async Task UpdateAnswerById_Returns_Updated_Answer_Id()
    {
        // Arrange
        var answerSeed = AnswerSeeds.AnswerToUpdate;
        var answerSeedModel = Mapper.Map<AnswerDetailModel>(answerSeed);
        var answerSeedModelUpdated = Mapper.Map<AnswerDetailModel>(answerSeed);
        answerSeedModelUpdated.Text = "Updated text";

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/answers/{answerSeedModel.Id}", answerSeedModelUpdated);
        var putId = await put.Content.ReadFromJsonAsync<IdModel>();
        var get = await Client.Value.GetAsync($"/api/answers/{putId!.Id}");
        var getId = (await get.Content.ReadFromJsonAsync<AnswerDetailModel>())!.Id;

        // Assert
        Assert.Equal(HttpStatusCode.OK, put.StatusCode);
        Assert.Equal(putId.Id, getId);
    }

    [Fact]
    public async Task UpdateAnswerByIdForNonExistentQuestion_Returns_BadRequest()
    {
        // Arrange
        var answerSeed = AnswerSeeds.AnswerToUpdate;
        var answerSeedModelUpdated = Mapper.Map<AnswerDetailModel>(answerSeed);
        answerSeedModelUpdated.QuestionId = Guid.NewGuid();

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/answers/{answerSeedModelUpdated.Id}",
            answerSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }

    [Fact]
    public async Task UpdateAnswerById_Returns_NotFound_When_Answer_Does_Not_Exist()
    {
        // Arrange
        var answerSeed = AnswerSeeds.AnswerToUpdate;
        var answerSeedModelUpdated = Mapper.Map<AnswerDetailModel>(answerSeed);
        answerSeedModelUpdated.Text = "Updated text";
        var nonExistentId = Guid.NewGuid();

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/answers/{nonExistentId}", answerSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);
    }

    [Fact]
    public async Task UpdateAnswerByIdWithGarbage_Returns_BadRequest()
    {
        // Arrange
        var answerSeed = AnswerSeeds.AnswerToUpdate;
        var answerSeedModelUpdated = Mapper.Map<AnswerDetailModel>(answerSeed);
        var garbage = new { Garbage = "Garbage" };

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/answers/{answerSeedModelUpdated.Id}", garbage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }

    [Fact]
    public async Task DeleteAnswerById_Returns_Ok()
    {
        // Arrange
        var answerSeed = AnswerSeeds.AnswerToDelete;
        var answerSeedModel = Mapper.Map<AnswerDetailModel>(answerSeed);

        // Act
        var delete = await Client.Value.DeleteAsync($"/api/answers/{answerSeedModel.Id}");
        var get = await Client.Value.GetAsync($"/api/answers/{answerSeedModel.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, delete.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task DeleteAnswerById_Returns_NotFound_When_Answer_Does_Not_Exist()
    {
        // Arrange
        var nonexistentId = Guid.NewGuid();

        // Act
        var delete = await Client.Value.DeleteAsync($"/api/answers/{nonexistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);
    }
}