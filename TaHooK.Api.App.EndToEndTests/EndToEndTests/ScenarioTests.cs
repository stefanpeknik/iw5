using System.Net;
using System.Net.Http.Json;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Quiz;
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
        var userSeedModel = mapper.Map<UserDetailModel>(userSeed);
        // score
        var scoreSeed = ScoreSeeds.DefaultScore;
        var scoreSeedModel = mapper.Map<ScoreDetailModel>(scoreSeed);
        // quiz
        var quizSeed = QuizSeeds.DefaultQuiz;
        var quizSeedModel = mapper.Map<QuizDetailModel>(quizSeed);
        // question
        var questionSeed = QuestionSeeds.DefaultQuestion;
        var questionSeedModel = mapper.Map<QuestionDetailModel>(questionSeed);
        // answers
        var answerSeed = AnswerSeeds.DefaultAnswer;
        var answerSeedModel1 = mapper.Map<AnswerDetailModel>(answerSeed);
        var answerSeedModel2 = mapper.Map<AnswerDetailModel>(answerSeed);
        
        
        // Act
        // user
        var userResponse = await client.Value.PostAsJsonAsync("/api/users", userSeedModel);
        userResponse.EnsureSuccessStatusCode();
        var userId = await userResponse.Content.ReadFromJsonAsync<Guid>();
        // quiz
        var quizResponse = await client.Value.PostAsJsonAsync("/api/quizzes", quizSeedModel);
        quizResponse.EnsureSuccessStatusCode();
        var quizId = await quizResponse.Content.ReadFromJsonAsync<Guid>();
        // question
        questionSeedModel.QuizId = quizId;
        var questionResponse = await client.Value.PostAsJsonAsync("/api/questions", questionSeedModel);
        questionResponse.EnsureSuccessStatusCode();
        var questionId = await questionResponse.Content.ReadFromJsonAsync<Guid>();
        // answers
        answerSeedModel1.QuestionId = questionId;
        answerSeedModel2.QuestionId = questionId;
        var answerResponse1 = await client.Value.PostAsJsonAsync("/api/answers", answerSeedModel1);
        answerResponse1.EnsureSuccessStatusCode();
        var answerId1 = await answerResponse1.Content.ReadFromJsonAsync<Guid>();
        var answerResponse2 = await client.Value.PostAsJsonAsync("/api/answers", answerSeedModel2);
        answerResponse2.EnsureSuccessStatusCode();
        var answerId2 = await answerResponse2.Content.ReadFromJsonAsync<Guid>();
        // score
        scoreSeedModel.QuizId = quizId;
        scoreSeedModel.UserId = userId;
        var scoreResponse = await client.Value.PostAsJsonAsync("/api/scores", scoreSeedModel);
        scoreResponse.EnsureSuccessStatusCode();
        var scoreId = await scoreResponse.Content.ReadFromJsonAsync<Guid>();
        
        
        // Assert
        // user
        var userGetResponse = await client.Value.GetAsync($"/api/users/{userId}");
        userGetResponse.EnsureSuccessStatusCode();
        var userGet = await userGetResponse.Content.ReadFromJsonAsync<UserDetailModel>();
        Assert.NotNull(userGet);
        Assert.Equal(userId, userGet.Id);
        // quiz
        var quizGetResponse = await client.Value.GetAsync($"/api/quizzes/{quizId}");
        quizGetResponse.EnsureSuccessStatusCode();
        var quizGet = await quizGetResponse.Content.ReadFromJsonAsync<QuizDetailModel>();
        Assert.NotNull(quizGet);
        Assert.Equal(quizId, quizGet.Id);
        // question
        var questionGetResponse = await client.Value.GetAsync($"/api/questions/{questionId}");
        questionGetResponse.EnsureSuccessStatusCode();
        var questionGet = await questionGetResponse.Content.ReadFromJsonAsync<QuestionDetailModel>();
        Assert.NotNull(questionGet);
        Assert.Equal(questionId, questionGet.Id);
        // answers
        var answerGetResponse1 = await client.Value.GetAsync($"/api/answers/{answerId1}");
        answerGetResponse1.EnsureSuccessStatusCode();
        var answerGet1 = await answerGetResponse1.Content.ReadFromJsonAsync<AnswerDetailModel>();
        Assert.NotNull(answerGet1);
        Assert.Equal(answerId1, answerGet1.Id);
        var answerGetResponse2 = await client.Value.GetAsync($"/api/answers/{answerId2}");
        answerGetResponse2.EnsureSuccessStatusCode();
        var answerGet2 = await answerGetResponse2.Content.ReadFromJsonAsync<AnswerDetailModel>();
        Assert.NotNull(answerGet2);
        Assert.Equal(answerId2, answerGet2.Id);
        // score
        var scoreGetResponse = await client.Value.GetAsync($"/api/scores/{scoreId}");
        scoreGetResponse.EnsureSuccessStatusCode();
        var scoreGet = await scoreGetResponse.Content.ReadFromJsonAsync<ScoreDetailModel>();
        Assert.NotNull(scoreGet);
        Assert.Equal(scoreId, scoreGet.Id);
    }
}
