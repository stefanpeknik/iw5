using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Score;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.App.Hubs;

public interface IQuizClient
{
    Task UsersInLobby(IEnumerable<UserListModel> users);
    Task NextQuestion(QuestionDetailModel? question);
    
    Task AnswerDistribution(List<AnswerDistributionModel> question);
    
    Task QuestionResult(QuestionResult result);

    Task QuizResult(List<ScoreListModel> result);
}