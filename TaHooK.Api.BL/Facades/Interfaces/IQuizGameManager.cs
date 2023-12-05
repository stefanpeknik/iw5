using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface IQuizGameManager
{
    IEnumerable<UserListModel> GetQuizUsers(Guid quizId);
    void AddUserToQuiz(Guid quizId, Guid userId);
}