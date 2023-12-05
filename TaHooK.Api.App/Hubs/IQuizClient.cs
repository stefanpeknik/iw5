using TaHooK.Common.Models.User;

namespace TaHooK.Api.App.Hubs;

public interface IQuizClient
{
    Task ReceiveMessage(string message);
    Task UsersInLobby(IEnumerable<UserListModel> users);
}