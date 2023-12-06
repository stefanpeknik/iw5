namespace TaHooK.Api.DAL.Repositories;

public class LiveLiveQuizStateRepository: ILiveQuizStateRepository
{
    private readonly LiveQuizStates _liveQuizStates;

    public LiveLiveQuizStateRepository(LiveQuizStates liveQuizStates)
    {
        _liveQuizStates = liveQuizStates;
    }
    public void AddUserToQuiz(Guid quizId, Guid userId)
    {
        if (_liveQuizStates.QuizesStates.ContainsKey(quizId) is false)
        {
            _liveQuizStates.QuizesStates.Add(quizId, new QuizState
            {
                QuizId = quizId,
                Users = new HashSet<Guid>(),
                CurrentQuestionIndex = 0
            });
        }

        _liveQuizStates.QuizesStates[quizId].Users.Add(userId);
    }
    
    public IEnumerable<Guid> GetQuizUsers(Guid quizId)
    {
        if (_liveQuizStates.QuizesStates.ContainsKey(quizId) is false)
        {
            return new List<Guid>();
        }

        return _liveQuizStates.QuizesStates[quizId].Users;
    }
    
    public void AddUserConnection(string connectionId, Guid userId)
    {
        _liveQuizStates.UserConnections.Add(connectionId, userId);
    }
    
    public QuizState GetQuizState(Guid quizId)
    {
        if (_liveQuizStates.QuizesStates.ContainsKey(quizId) is false)
        {
            return null;
        }

        return _liveQuizStates.QuizesStates[quizId];
    }
    
    public Guid GetUserConnection(string connectionId)
    {
        return _liveQuizStates.UserConnections[connectionId];
    }
    
    public void RemoveUserConnection(string connectionId)
    {
        _liveQuizStates.UserConnections.Remove(connectionId);
    }
    
    public Guid? GetUserQuiz(Guid userId)
    {
        foreach (var quiz in _liveQuizStates.QuizesStates)
        {
            if (quiz.Value.Users.Contains(userId))
            {
                return quiz.Key;
            }
        }

        return null;
    }
    
    public void RemoveUserFromQuiz(Guid quizId, Guid userId)
    {
        if (_liveQuizStates.QuizesStates.ContainsKey(quizId) is false)
        {
            return;
        }

        _liveQuizStates.QuizesStates[quizId].Users.Remove(userId);
    }
}