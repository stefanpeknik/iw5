using System.Collections.ObjectModel;
using TaHooK.Common.Models.Answer;

namespace TaHooK.Common.Models.Question
{
    public record QuestionDetailModel : IWithId
    {
        public Guid Id { get; init; }
        public required string Text { get; set; }

        public ObservableCollection<AnswerListModel> Answers { get; set; } = new();

        public required Guid QuizId { get; set; }
    }
}
