using System.Collections.ObjectModel;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Score;

namespace TaHooK.Common.Models.Quiz
{
    public record QuizDetailModel : IWithId
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required DateTime Schedule { get; set; }
        public required bool Finished { get; set; }

        public ICollection<QuestionListModel> Questions { get; set; } = new List<QuestionListModel>();

        public ICollection<ScoreListModel> Scores { get; set; } = new List<ScoreListModel>();
    }
}
