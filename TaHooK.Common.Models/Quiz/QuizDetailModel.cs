using System.Collections.ObjectModel;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Score;

namespace TaHooK.Common.Models.Quiz
{
    public record QuizDetailModel : IWithId
    {
        public Guid Id { get; init; }
        public required string Title { get; set; }
        public required DateTime Schedule { get; set; }

        public ObservableCollection<QuestionListModel> Questions { get; set; } = new();

        public ObservableCollection<ScoreListModel> Scores { get; set; } = new();
    }
}
