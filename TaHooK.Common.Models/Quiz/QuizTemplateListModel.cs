namespace TaHooK.Common.Models.Quiz;

public record QuizTemplateListModel : IWithId
{
    public required string Title { get; set; }
    public Guid Id { get; set; }
}