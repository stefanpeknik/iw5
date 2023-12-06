namespace TaHooK.Api.DAL;

public class LiveQuizStates
{
    public Dictionary<Guid, QuizState> QuizesStates = new Dictionary<Guid, QuizState>();
    public Dictionary<string, Guid> UserConnections = new Dictionary<string, Guid>();
}

public record QuizState
{
    public Guid QuizId { get; init; }
    public HashSet<Guid> Users { get; init; } = new HashSet<Guid>();
    public int CurrentQuestionIndex { get; init; }
}