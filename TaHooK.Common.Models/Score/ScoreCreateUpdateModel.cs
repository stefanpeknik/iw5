namespace TaHooK.Common.Models.Score;

public record ScoreCreateUpdateModel
{
    public int Score { get; set; }

    public required Guid UserId { get; set; }

    public required Guid QuizId { get; set; }
}