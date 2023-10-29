namespace TaHooK.Common.Models.Question;

public record QuestionCreateUpdateModel
{
    public required string Text { get; set; }
    public required Guid QuizId { get; set; }
};