using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface IQuizGameManager
{
    IEnumerable<UserListModel> GetQuizUsers(Guid quizId);
    void AddUserToQuiz(Guid quizId, Guid userId);
    void AddUserConnection(string connectionId, Guid userId);
    Guid GetUserConnection(string connectionId);
    void RemoveUserFromQuiz(Guid quizId, Guid userId);
    void RemoveUserConnection(string connectionId);
    Guid? GetUserQuiz(Guid userId);
}