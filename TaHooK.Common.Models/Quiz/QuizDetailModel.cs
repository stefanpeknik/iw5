using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Score;

namespace TaHooK.Common.Models.Quiz;

public record QuizDetailModel : IWithId
{
    public required string Title { get; set; }
    public required DateTime StartedAt { get; set; }
    public required bool Finished { get; set; }

    public required Guid TemplateId { get; set; }

    public ICollection<ScoreListModel> Scores { get; set; } = new List<ScoreListModel>();
    public Guid Id { get; set; }
}