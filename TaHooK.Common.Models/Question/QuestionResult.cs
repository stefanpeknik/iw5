using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Score;

namespace TaHooK.Common.Models.Question;

public record QuestionResult
{
    public List<AnswerDistributionModel> AnswerDistribution { get; init; } = new List<AnswerDistributionModel>();
    public List<ScoreListModel> Results { get; init; } = new List<ScoreListModel>();
}