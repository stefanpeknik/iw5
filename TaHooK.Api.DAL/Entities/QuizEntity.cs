using AutoMapper;

namespace TaHooK.Api.DAL.Entities;

public record QuizEntity : EntityBase
{
    public required DateTime Schedule { get; set; }

    public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();

    public ICollection<ScoreEntity> Scores { get; set; } = new List<ScoreEntity>();
    
    
    public class QuizEntityMapperProfile : Profile
    {
        public QuizEntityMapperProfile()
        {
            CreateMap<QuizEntity, QuizEntity>();
        }
    }
}
