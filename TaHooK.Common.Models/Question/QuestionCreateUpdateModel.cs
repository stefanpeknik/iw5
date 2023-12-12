using TaHooK.Common.Models.Answer;

namespace TaHooK.Common.Models.Question;

public record QuestionCreateUpdateModel
{
    public required string Text { get; set; }
    public required Guid QuizTemplateId { get; set; }
    
    public List<AnswerListModel> Answers { get; set; }
}