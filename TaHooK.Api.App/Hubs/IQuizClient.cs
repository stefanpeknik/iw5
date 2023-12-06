using TaHooK.Common.Models.User;

namespace TaHooK.Api.App.Hubs;

public interface IQuizClient
{
    Task UsersInLobby(IEnumerable<UserListModel> users);
}