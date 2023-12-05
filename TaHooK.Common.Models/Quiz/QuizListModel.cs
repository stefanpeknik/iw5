namespace TaHooK.Common.Models.Quiz;

public record QuizListModel : IWithId
{
    public required string Title { get; set; }
    public required DateTime StartedAt { get; set; }
    public required bool Finished { get; set; }
    public Guid Id { get; set; }
}