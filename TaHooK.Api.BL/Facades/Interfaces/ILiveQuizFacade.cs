using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Score;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface ILiveQuizFacade
{
    IEnumerable<UserListModel> GetQuizUsers(Guid quizId);
    void AddUserToQuiz(Guid quizId, Guid userId);
    void AddUserConnection(string connectionId, Guid userId);
    Guid GetUserConnection(string connectionId);
    string? GetUserConnectionId(Guid userId);
    void RemoveUserFromQuiz(Guid quizId, Guid userId);
    void RemoveUserConnection(string connectionId);
    Guid? GetUserQuiz(Guid userId);
    Task<QuestionDetailModel?> GetNextQuestion(Guid quizId);

    void AnswerQuestion(Guid quizId, Guid userId, Guid answerId);
    Task<List<AnswerDistributionModel>> GetAnswerDistribution(Guid quizId);

    Task InitializeQuiz(Guid quizId);

    Task<List<ScoreListModel>> CalculateResult(Guid quizId);
}