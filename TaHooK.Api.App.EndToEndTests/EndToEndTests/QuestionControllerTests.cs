using System.Net;
using System.Net.Http.Json;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Responses;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class QuestionControllerTests : EndToEndTestsBase
{
    [Fact]
    public async Task GetAllQuestions_Returns_At_Least_One_Question()
    {
        // Arrange
        var questionSeed = QuestionSeeds.DefaultQuestion;
        var questionSeedModel = Mapper.Map<QuestionListModel>(questionSeed);

        // Act
        var response = await Client.Value.GetAsync("/api/questions");
        response.EnsureSuccessStatusCode();
        var questions = await response.Content.ReadFromJsonAsync<ICollection<QuestionListModel>>();

        // Assert
        Assert.NotNull(questions);
        Assert.NotEmpty(questions);
        Assert.Contains(questions, question => question.Id == questionSeedModel.Id);
    }

    [Fact]
    public async Task GetQuestionById_Returns_Question_With_The_Same_Id()
    {
        // Arrange
        var questionSeed = QuestionSeeds.DefaultQuestion;
        var questionSeedModel = Mapper.Map<QuestionDetailModel>(questionSeed);

        // Act
        var response = await Client.Value.GetAsync($"/api/questions/{questionSeedModel.Id}");
        var question = await response.Content.ReadFromJsonAsync<QuestionDetailModel>();

        // Assert
        Assert.NotNull(question);
        Assert.Equal(questionSeedModel.Id, question.Id);
    }

    [Fact]
    public async Task GetQuestionById_Returns_NotFound_When_Question_Does_Not_Exist()
    {
        // Act
        var response = await Client.Value.GetAsync($"/api/questions/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateQuestion_Returns_Created_Question_Id()
    {
        // Arrange
        var questionSeed = QuestionSeeds.DefaultQuestion;
        var questionSeedModel = Mapper.Map<QuestionCreateUpdateModel>(questionSeed);

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/questions", questionSeedModel);
        var postId = await post.Content.ReadFromJsonAsync<IdModel>();
        var get = await Client.Value.GetAsync($"/api/questions/{postId?.Id}");
        var getId = (await get.Content.ReadFromJsonAsync<QuestionDetailModel>())!.Id;

        // Assert
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        Assert.Equal(postId?.Id, getId);
    }

    [Fact]
    public async Task CreateQuestionForNonExistentQuestion_Returns_BadRequest()
    {
        // Arrange
        var questionSeed = QuestionSeeds.DefaultQuestion;
        var questionSeedModel = Mapper.Map<QuestionDetailModel>(questionSeed);
        questionSeedModel.QuizId = Guid.NewGuid();

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/questions", questionSeedModel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }

    [Fact]
    public async Task CreateGarbage_Returns_BadRequest()
    {
        // Arrange
        var garbage = new { Garbage = "Garbage" };

        // Act
        var post = await Client.Value.PostAsJsonAsync("/api/questions", garbage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, post.StatusCode);
    }

    [Fact]
    public async Task UpdateQuestionById_Returns_Updated_Question_Id()
    {
        // Arrange
        var questionSeed = QuestionSeeds.QuestionToUpdate;
        var questionSeedModel = Mapper.Map<QuestionDetailModel>(questionSeed);
        var questionSeedModelUpdated = Mapper.Map<QuestionCreateUpdateModel>(questionSeed);
        questionSeedModelUpdated.Text = "Updated text";

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/questions/{questionSeedModel.Id}",
            questionSeedModelUpdated);
        var putId = await put.Content.ReadFromJsonAsync<IdModel>();
        var get = await Client.Value.GetAsync($"/api/questions/{putId?.Id}");
        var getId = (await get.Content.ReadFromJsonAsync<QuestionDetailModel>())!.Id;

        // Assert
        Assert.Equal(HttpStatusCode.OK, put.StatusCode);
        Assert.Equal(putId?.Id, getId);
    }

    [Fact]
    public async Task UpdateQuestionByIdForNonExistentQuestion_Returns_BadRequest()
    {
        // Arrange
        var questionSeed = QuestionSeeds.QuestionToUpdate;
        var questionSeedModelUpdated = Mapper.Map<QuestionDetailModel>(questionSeed);
        questionSeedModelUpdated.QuizId = Guid.NewGuid();

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/questions/{questionSeedModelUpdated.Id}",
            questionSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }

    [Fact]
    public async Task UpdateQuestionById_Returns_NotFound_When_Question_Does_Not_Exist()
    {
        // Arrange
        var questionSeed = QuestionSeeds.QuestionToUpdate;
        var questionSeedModelUpdated = Mapper.Map<QuestionDetailModel>(questionSeed);
        questionSeedModelUpdated.Text = "Updated text";
        var nonExistentId = Guid.NewGuid();

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/questions/{nonExistentId}", questionSeedModelUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);
    }

    [Fact]
    public async Task UpdateQuestionByIdWithGarbage_Returns_BadRequest()
    {
        // Arrange
        var questionSeed = QuestionSeeds.QuestionToUpdate;
        var questionSeedModelUpdated = Mapper.Map<QuestionDetailModel>(questionSeed);
        var garbage = new { Garbage = "Garbage" };

        // Act
        var put = await Client.Value.PutAsJsonAsync($"/api/questions/{questionSeedModelUpdated.Id}", garbage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }

    [Fact]
    public async Task DeleteQuestionById_Returns_Ok()
    {
        // Arrange
        var questionSeed = QuestionSeeds.QuestionToDelete;
        var questionSeedModel = Mapper.Map<QuestionDetailModel>(questionSeed);

        // Act
        var delete = await Client.Value.DeleteAsync($"/api/questions/{questionSeedModel.Id}");
        var get = await Client.Value.GetAsync($"/api/questions/{questionSeedModel.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, delete.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task DeleteQuestionById_Returns_NotFound_When_Question_Does_Not_Exist()
    {
        // Arrange
        var nonexistentId = Guid.NewGuid();

        // Act
        var delete = await Client.Value.DeleteAsync($"/api/questions/{nonexistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);
    }
}