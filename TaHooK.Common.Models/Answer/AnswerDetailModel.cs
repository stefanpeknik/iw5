using TaHooK.Common.Enums;

namespace TaHooK.Common.Models.Answer;

public record AnswerDetailModel : IWithId
{
    public required AnswerType Type { get; set; }
    public required string Text { get; set; }
    public Uri? Picture { get; set; }
    public required bool IsCorrect { get; set; }

    public required Guid QuestionId { get; set; }
    public Guid Id { get; set; }
}