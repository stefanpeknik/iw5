namespace TaHooK.Common.Models.Quiz;

public record QuizCreateUpdateModel
{
    public required string Title { get; set; }
    public required DateTime StartedAt { get; set; }
    public required bool Finished { get; set; }
    public required Guid TemplateId { get; set; }
}