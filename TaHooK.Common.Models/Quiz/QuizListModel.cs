namespace TaHooK.Common.Models.Quiz;

public record QuizListModel : IWithId
{
    public required string Title { get; set; }
    public required DateTime StartedAt { get; set; }
    public required bool Finished { get; set; }
    
    public required Guid CreatorId { get; set; }
    
    public string CreatorName { get; set; } = string.Empty;
    public Guid Id { get; set; }
}