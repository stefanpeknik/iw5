namespace TaHooK.Common.Models.Score;

public record ScoreDetailModel : IWithId
{
    public int Score { get; set; }

    public required Guid UserId { get; set; }

    public required Guid QuizId { get; set; }
    public Guid Id { get; set; }
}