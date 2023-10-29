using TaHooK.Common.Models.Answer;

namespace TaHooK.Common.Models.Question;

public record QuestionDetailModel : IWithId
{
    public required string Text { get; set; }

    public ICollection<AnswerListModel> Answers { get; set; } = new List<AnswerListModel>();

    public required Guid QuizId { get; set; }
    public Guid Id { get; set; }
}