using AutoMapper;

namespace TaHooK.Api.DAL.Entities;

public record QuizTemplateEntity : EntityBase
{
    public required string Title { get; set; }

    public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();

    public class QuizTemplateEntityMapperProfile : Profile
    {
        public QuizTemplateEntityMapperProfile()
        {
            CreateMap<QuizTemplateEntity, QuizTemplateEntity>();
        }
    }
}