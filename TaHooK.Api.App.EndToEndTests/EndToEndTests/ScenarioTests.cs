using System.Net.Http.Json;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Responses;
using TaHooK.Common.Models.Score;
using TaHooK.Common.Models.User;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class ScenarioTests : EndToEndTestsBase
{
    [Fact]
    public async Task User_Creates_Quiz_Question_Two_Answers_And_Scores_In_It()
    {
        // Arrange
        // user
        var userSeed = UserSeeds.DefaultUser;
        var userSeedModel = Mapper.Map<UserCreateUpdateModel>(userSeed);
        // score
        var scoreSeed = ScoreSeeds.DefaultScore;
        var scoreSeedModel = Mapper.Map<ScoreCreateUpdateModel>(scoreSeed);
        // quiz
        var quizSeed = QuizSeeds.DefaultQuiz;
        var quizSeedModel = Mapper.Map<QuizCreateUpdateModel>(quizSeed);
        // question
        var questionSeed = QuestionSeeds.DefaultQuestion;
        var questionSeedModel = Mapper.Map<QuestionCreateUpdateModel>(questionSeed);
        // answers
        var answerSeed = AnswerSeeds.DefaultAnswer;
        var answerSeedModel1 = Mapper.Map<AnswerCreateUpdateModel>(answerSeed);
        var answerSeedModel2 = Mapper.Map<AnswerCreateUpdateModel>(answerSeed);


        // Act
        // user
        var userResponse = await Client.Value.PostAsJsonAsync("/api/users", userSeedModel);
        userResponse.EnsureSuccessStatusCode();
        var userId = await userResponse.Content.ReadFromJsonAsync<IdModel>();
        // quiz
        var quizResponse = await Client.Value.PostAsJsonAsync("/api/quizzes", quizSeedModel);
        quizResponse.EnsureSuccessStatusCode();
        var quizId = await quizResponse.Content.ReadFromJsonAsync<IdModel>();
        // question
        questionSeedModel.QuizId = quizId!.Id;
        var questionResponse = await Client.Value.PostAsJsonAsync("/api/questions", questionSeedModel);
        questionResponse.EnsureSuccessStatusCode();
        var questionId = await questionResponse.Content.ReadFromJsonAsync<IdModel>();
        // answers
        answerSeedModel1.QuestionId = questionId!.Id;
        answerSeedModel2.QuestionId = questionId.Id;
        var answerResponse1 = await Client.Value.PostAsJsonAsync("/api/answers", answerSeedModel1);
        answerResponse1.EnsureSuccessStatusCode();
        var answerId1 = await answerResponse1.Content.ReadFromJsonAsync<IdModel>();
        var answerResponse2 = await Client.Value.PostAsJsonAsync("/api/answers", answerSeedModel2);
        answerResponse2.EnsureSuccessStatusCode();
        var answerId2 = await answerResponse2.Content.ReadFromJsonAsync<IdModel>();
        // score
        scoreSeedModel.QuizId = quizId.Id;
        scoreSeedModel.UserId = userId!.Id;
        var scoreResponse = await Client.Value.PostAsJsonAsync("/api/scores", scoreSeedModel);
        scoreResponse.EnsureSuccessStatusCode();
        var scoreId = await scoreResponse.Content.ReadFromJsonAsync<IdModel>();


        // Assert
        // user
        var userGetResponse = await Client.Value.GetAsync($"/api/users/{userId.Id}");
        userGetResponse.EnsureSuccessStatusCode();
        var userGet = await userGetResponse.Content.ReadFromJsonAsync<UserDetailModel>();
        Assert.NotNull(userGet);
        Assert.Equal(userId.Id, userGet.Id);
        // quiz
        var quizGetResponse = await Client.Value.GetAsync($"/api/quizzes/{quizId.Id}");
        quizGetResponse.EnsureSuccessStatusCode();
        var quizGet = await quizGetResponse.Content.ReadFromJsonAsync<QuizDetailModel>();
        Assert.NotNull(quizGet);
        Assert.Equal(quizId.Id, quizGet.Id);
        // question
        var questionGetResponse = await Client.Value.GetAsync($"/api/questions/{questionId.Id}");
        questionGetResponse.EnsureSuccessStatusCode();
        var questionGet = await questionGetResponse.Content.ReadFromJsonAsync<QuestionDetailModel>();
        Assert.NotNull(questionGet);
        Assert.Equal(questionId.Id, questionGet.Id);
        // answers
        var answerGetResponse1 = await Client.Value.GetAsync($"/api/answers/{answerId1?.Id}");
        answerGetResponse1.EnsureSuccessStatusCode();
        var answerGet1 = await answerGetResponse1.Content.ReadFromJsonAsync<AnswerDetailModel>();
        Assert.NotNull(answerGet1);
        Assert.Equal(answerId1?.Id, answerGet1.Id);
        var answerGetResponse2 = await Client.Value.GetAsync($"/api/answers/{answerId2?.Id}");
        answerGetResponse2.EnsureSuccessStatusCode();
        var answerGet2 = await answerGetResponse2.Content.ReadFromJsonAsync<AnswerDetailModel>();
        Assert.NotNull(answerGet2);
        Assert.Equal(answerId2?.Id, answerGet2.Id);
        // score
        var scoreGetResponse = await Client.Value.GetAsync($"/api/scores/{scoreId?.Id}");
        scoreGetResponse.EnsureSuccessStatusCode();
        var scoreGet = await scoreGetResponse.Content.ReadFromJsonAsync<ScoreDetailModel>();
        Assert.NotNull(scoreGet);
        Assert.Equal(scoreId?.Id, scoreGet.Id);
    }
}