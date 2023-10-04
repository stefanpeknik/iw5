namespace TaHooK.Common.Models.Quiz
{
    public record QuizListModel : IWithId
    {
        public Guid Id { get; init; }
        public required string Title { get; set; }
        public required DateTime Schedule { get; set; }
        public required bool Finished { get; set; }
    }
}
