namespace TaHooK.Common.Models.Score
{
    public record ScoreDetailModel : IWithId
    {
        public Guid Id { get; init; }
        public int Score { get; set; }

        public required Guid UserId { get; set; }

        public required Guid QuizId { get; set; }
    }
}
