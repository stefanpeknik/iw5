namespace TaHooK.Common.Models.Question
{
    public record QuestionListModel : IWithId
    {
        public Guid Id { get; init; }
        public required string Text { get; set; }
    }
}
