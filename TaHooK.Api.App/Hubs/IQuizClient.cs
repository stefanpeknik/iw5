using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.App.Hubs;

public interface IQuizClient
{
    Task UsersInLobby(IEnumerable<UserListModel> users);
    Task NextQuestion(QuestionDetailModel? question);
}