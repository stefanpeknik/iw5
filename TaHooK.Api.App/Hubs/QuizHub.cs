using Microsoft.AspNetCore.SignalR;
using TaHooK.Api.BL.Facades;
using TaHooK.Api.BL.Facades.Interfaces;

namespace TaHooK.Api.App.Hubs;

public class QuizHub: Hub<IQuizClient>
{
    private readonly IQuizGameManager _quizManager;

    public QuizHub(IQuizGameManager quizManager)
    {
        _quizManager = quizManager;
    }
    
    public async Task SendMessage(string message)
    {
        await Clients.All.ReceiveMessage(message);
    }
    
    public async Task JoinQuiz(Guid quizId, Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, quizId.ToString());
        _quizManager.AddUserToQuiz(quizId, userId);
        
        var quizUsers = await _quizManager.GetQuizUsers(quizId);
        
        await Clients.Group(quizId.ToString()).UsersInLobby(quizUsers);
    }
}