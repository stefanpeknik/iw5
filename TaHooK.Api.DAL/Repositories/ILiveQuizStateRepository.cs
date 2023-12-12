namespace TaHooK.Api.DAL.Repositories;

public interface ILiveQuizStateRepository
{
    void AddUserToQuiz(Guid quizId, Guid userId);
    public IEnumerable<Guid> GetQuizUsers(Guid quizId);
    void AddUserConnection(string connectionId, Guid userId);
    Guid GetUserConnection(string connectionId);
    string? GetUserConnectionId(Guid userId);

    void RemoveUserFromQuiz(Guid quizId, Guid userId);

    void RemoveUserConnection(string connectionId);

    Guid? GetUserQuiz(Guid userId);

    QuizState GetQuizState(Guid quizId);

    void AnswerQuestion(Guid quizId, Guid userId, Guid answerId);
}