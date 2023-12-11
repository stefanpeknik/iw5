using TaHooK.Api.DAL.Entities;

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
    
    public List<UserAnswer> UsersAnswers { get; init; } = new List<UserAnswer>();
    public Guid CurrentQuestionId { get; set; }
    public int NextQuestionIndex { get; set; }
}

public record UserAnswer
{
    public Guid UserId { get; init; }
    public Guid AnswerId { get; init; }
    public DateTime AnswerTime { get; init; }
}