using Microsoft.AspNetCore.SignalR;
using TaHooK.Api.BL.Facades.Interfaces;

namespace TaHooK.Api.App.Hubs;

public class QuizHub: Hub<IQuizClient>
{
    private readonly ILiveQuizFacade _liveQuizManager;

    public QuizHub(ILiveQuizFacade liveQuizManager)
    {
        _liveQuizManager = liveQuizManager;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userId = httpContext?.Request.Query["userId"].ToString();
        
        if (userId != null)
        {
            _liveQuizManager.AddUserConnection(Context.ConnectionId, Guid.Parse(userId));
        }
        
        // Use the userId as needed
        await base.OnConnectedAsync();
    }
    
    public async Task JoinQuiz(Guid quizId)
    {
        var userId = _liveQuizManager.GetUserConnection(Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, quizId.ToString());
        _liveQuizManager.AddUserToQuiz(quizId, userId);
        
        var quizUsers = _liveQuizManager.GetQuizUsers(quizId);
        
        await Clients.Group(quizId.ToString()).UsersInLobby(quizUsers);
    }
    
    public async Task LeaveQuiz(Guid quizId)
    {
        var userId = _liveQuizManager.GetUserConnection(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, quizId.ToString());
        _liveQuizManager.RemoveUserFromQuiz(quizId, userId);
        
        var quizUsers = _liveQuizManager.GetQuizUsers(quizId);
        
        await Clients.Group(quizId.ToString()).UsersInLobby(quizUsers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = _liveQuizManager.GetUserConnection(Context.ConnectionId);
        var quizId = _liveQuizManager.GetUserQuiz(userId);
        if (quizId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, quizId.ToString());
            _liveQuizManager.RemoveUserFromQuiz(quizId.Value, userId);
            var quizUsers = _liveQuizManager.GetQuizUsers(quizId.Value);
            await Clients.Group(quizId.ToString()).UsersInLobby(quizUsers);
        }
        _liveQuizManager.RemoveUserConnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task StartQuiz(Guid quizId)
    {
        // TODO: update quiz startedAt   
        var question = await _liveQuizManager.GetNextQuestion(quizId);
        if (question != null)
        {
            await Clients.Group(quizId.ToString()).NextQuestion(question);
        }
    }
    
    public async Task GetNextQuestion(Guid quizId)
    {
        var question = await _liveQuizManager.GetNextQuestion(quizId);
        if (question != null)
        {
            await Clients.Group(quizId.ToString()).NextQuestion(question);
        }
        else
        {
            await Clients.Client(Context.ConnectionId).NextQuestion(null);
        }
    }
}