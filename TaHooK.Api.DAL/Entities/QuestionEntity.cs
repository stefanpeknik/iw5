using AutoMapper;
using TaHooK.Common.Extensions;

namespace TaHooK.Api.DAL.Entities;

public record QuestionEntity : EntityBase
{
    public required string Text { get; set; }

    public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

    public required Guid QuizTemplateId { get; set; }
    public QuizTemplateEntity? QuizTemplate { get; set; }


    public class QuestionEntityMapperProfile : Profile
    {
        public QuestionEntityMapperProfile()
        {
            CreateMap<QuestionEntity, QuestionEntity>()
                .Ignore(dst => dst.QuizTemplate);
        }
    }
}