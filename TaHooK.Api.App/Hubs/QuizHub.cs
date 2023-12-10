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
        
        if (userId != null && userId != Guid.Empty.ToString())
        {
            _liveQuizManager.AddUserConnection(Context.ConnectionId, Guid.Parse(userId));
        }
        
        // Use the userId as needed
        await base.OnConnectedAsync();
    }
    
    public async Task JoinQuiz(Guid quizId)
    {
        var userId = _liveQuizManager.GetUserConnection(Context.ConnectionId);
        if (userId == Guid.Empty)
        {
            return;
        }
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

        if (userId != Guid.Empty)
        {
            var quizId = _liveQuizManager.GetUserQuiz(userId);
            if (quizId != null)
            {
                _liveQuizManager.RemoveUserFromQuiz(quizId.Value, userId);
                var quizUsers = _liveQuizManager.GetQuizUsers(quizId.Value);
                await Clients.Group(quizId.ToString()).UsersInLobby(quizUsers);
            }

            _liveQuizManager.RemoveUserConnection(Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task StartQuiz(Guid quizId)
    {
        await _liveQuizManager.InitializeQuiz(quizId);
        var question = await _liveQuizManager.GetNextQuestion(quizId);
        if (question != null)
        {
            await Clients.Group(quizId.ToString()).NextQuestion(question);
        }
    }
    
    public async Task GetResults(Guid quizId)
    {
        var results = await _liveQuizManager.CalculateResult(quizId);
        await Clients.Client(Context.ConnectionId).QuizResults(results);
    }
    
    public async Task GetNextQuestion(Guid quizId)
    {
        var question = await _liveQuizManager.GetNextQuestion(quizId);
        
        // clear answered group
        var users = _liveQuizManager.GetQuizUsers(quizId);
        var answeredGroup = $"{quizId}-answered";
        foreach (var user in users)
        {
            var userConnectionId = _liveQuizManager.GetUserConnectionId(user.Id);
            if (userConnectionId == null)
            {
                continue;
            }
            await Groups.RemoveFromGroupAsync(userConnectionId, answeredGroup);
        }
        
        if (question != null)
        {
            await Clients.Group(quizId.ToString()).NextQuestion(question);
        }
        else
        {
            await Clients.Client(Context.ConnectionId).NextQuestion(null);
        }
    }

    public async Task AnswerQuestion(Guid quizId, Guid answerId)
    {
        var answeredGroup = $"{quizId}-answered";
        var userId = _liveQuizManager.GetUserConnection(Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, answeredGroup);
        _liveQuizManager.AnswerQuestion(quizId, userId, answerId);
        
        var answerDistribution = await _liveQuizManager.GetAnswerDistribution(quizId);
        
        await Clients.Group(answeredGroup).AnswerDistribution(answerDistribution);
    }
}