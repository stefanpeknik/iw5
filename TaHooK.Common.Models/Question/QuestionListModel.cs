namespace TaHooK.Common.Models.Question;

public record QuestionListModel : IWithId
{
    public required string Text { get; set; }
    public Guid Id { get; set; }
}