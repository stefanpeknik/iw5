namespace TaHooK.Api.DAL.Repositories;

public class QuizGameStateRepository: IQuizGameStateRepository
{
    private readonly QuizGameState _quizGameState;

    public QuizGameStateRepository(QuizGameState quizGameState)
    {
        _quizGameState = quizGameState;
    }
    public void AddUserToQuiz(Guid quizId, Guid userId)
    {
        if (_quizGameState.QuizesUsers.ContainsKey(quizId) is false)
        {
            _quizGameState.QuizesUsers.Add(quizId, new HashSet<Guid>());
        }

        _quizGameState.QuizesUsers[quizId].Add(userId);
    }
    
    public IEnumerable<Guid> GetQuizUsers(Guid quizId)
    {
        if (_quizGameState.QuizesUsers.ContainsKey(quizId) is false)
        {
            return new List<Guid>();
        }

        return _quizGameState.QuizesUsers[quizId];
    }
    
    public void AddUserConnection(string connectionId, Guid userId)
    {
        _quizGameState.UserConnections.Add(connectionId, userId);
    }
    
    public Guid GetUserConnection(string connectionId)
    {
        return _quizGameState.UserConnections[connectionId];
    }
    
    public void RemoveUserConnection(string connectionId)
    {
        _quizGameState.UserConnections.Remove(connectionId);
    }
    
    public Guid? GetUserQuiz(Guid userId)
    {
        foreach (var quiz in _quizGameState.QuizesUsers)
        {
            if (quiz.Value.Contains(userId))
            {
                return quiz.Key;
            }
        }

        return null;
    }
    
    public void RemoveUserFromQuiz(Guid quizId, Guid userId)
    {
        if (_quizGameState.QuizesUsers.ContainsKey(quizId) is false)
        {
            return;
        }

        _quizGameState.QuizesUsers[quizId].Remove(userId);
    }
}