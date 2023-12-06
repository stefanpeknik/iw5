namespace TaHooK.Api.DAL.Repositories;

public interface IQuizGameStateRepository
{
    void AddUserToQuiz(Guid quizId, Guid userId);
    public IEnumerable<Guid> GetQuizUsers(Guid quizId);
    void AddUserConnection(string connectionId, Guid userId);
    Guid GetUserConnection(string connectionId);

    void RemoveUserFromQuiz(Guid quizId, Guid userId);

    void RemoveUserConnection(string connectionId);

    Guid? GetUserQuiz(Guid userId);
}