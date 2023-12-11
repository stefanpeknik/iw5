using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Score;

namespace TaHooK.Common.Models.Quiz;

public record QuizTemplateDetailModel : IWithId
{
    public required string Title { get; set; }
    public ICollection<QuestionListModel> Questions { get; set; } = new List<QuestionListModel>();
    
    public required Guid CreatorId { get; set; }
    
    public string CreatorName { get; set; } = string.Empty;
    
    public Guid Id { get; set; }
}