namespace TaHooK.Api.DAL.Repositories;

public interface IQuizGameStateRepository
{
    void AddUserToQuiz(Guid quizId, Guid userId);
    public IEnumerable<Guid> GetQuizUsers(Guid quizId);
}