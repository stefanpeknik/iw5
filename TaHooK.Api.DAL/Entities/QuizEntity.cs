using AutoMapper;

namespace TaHooK.Api.DAL.Entities;

public record QuizEntity : EntityBase
{
    public required Guid TemplateId { get; set; }
    public required QuizTemplateEntity Template { get; set; }
    public required DateTime StartedAt { get; set; }
    public required bool Finished { get; set; }
    public ICollection<ScoreEntity> Scores { get; set; } = new List<ScoreEntity>();


    public class QuizEntityMapperProfile : Profile
    {
        public QuizEntityMapperProfile()
        {
            CreateMap<QuizEntity, QuizEntity>();
        }
    }
}