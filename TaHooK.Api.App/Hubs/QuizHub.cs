using Microsoft.AspNetCore.SignalR;
using TaHooK.Api.BL.Facades.Interfaces;

namespace TaHooK.Api.App.Hubs;

public class QuizHub: Hub<IQuizClient>
{
    private readonly IQuizGameManager _quizManager;

    public QuizHub(IQuizGameManager quizManager)
    {
        _quizManager = quizManager;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userId = httpContext?.Request.Query["userId"].ToString();
        
        if (userId != null)
        {
            _quizManager.AddUserConnection(Context.ConnectionId, Guid.Parse(userId));
        }
        
        // Use the userId as needed
        await base.OnConnectedAsync();
    }
    
    public async Task JoinQuiz(Guid quizId)
    {
        var userId = _quizManager.GetUserConnection(Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, quizId.ToString());
        _quizManager.AddUserToQuiz(quizId, userId);
        
        var quizUsers = _quizManager.GetQuizUsers(quizId);
        
        await Clients.Group(quizId.ToString()).UsersInLobby(quizUsers);
    }
    
    public async Task LeaveQuiz(Guid quizId)
    {
        var userId = _quizManager.GetUserConnection(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, quizId.ToString());
        _quizManager.RemoveUserFromQuiz(quizId, userId);
        
        var quizUsers = _quizManager.GetQuizUsers(quizId);
        
        await Clients.Group(quizId.ToString()).UsersInLobby(quizUsers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = _quizManager.GetUserConnection(Context.ConnectionId);
        var quizId = _quizManager.GetUserQuiz(userId);
        if (quizId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, quizId.ToString());
            _quizManager.RemoveUserFromQuiz(quizId.Value, userId);
            var quizUsers = _quizManager.GetQuizUsers(quizId.Value);
            await Clients.Group(quizId.ToString()).UsersInLobby(quizUsers);
        }
        _quizManager.RemoveUserConnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}