namespace TaHooK.Api.DAL.Common.Entities;

public record QuizEntity : EntityBase
{
    public required DateTime Schedule { get; set; }

    public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();

    public ICollection<ScoreEntity> Scores { get; set; } = new List<ScoreEntity>();
}
