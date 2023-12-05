using TaHooK.Api.DAL.Entities;

namespace TaHooK.Api.DAL;

public class QuizGameState
{
    public Dictionary<Guid, HashSet<Guid>> QuizesUsers = new Dictionary<Guid, HashSet<Guid>>();
}