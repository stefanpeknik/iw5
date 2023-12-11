namespace TaHooK.Common.Models.Quiz;

public record QuizTemplateCreateUpdateModel
{
    public required string Title { get; set; }
    
    public Guid? CreatorId { get; set; }
}