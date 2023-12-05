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
}