namespace TaHooK.Api.DAL;

public class QuizGameState
{
    public Dictionary<Guid, HashSet<Guid>> QuizesUsers = new Dictionary<Guid, HashSet<Guid>>();
    public Dictionary<string, Guid> UserConnections = new Dictionary<string, Guid>();
}