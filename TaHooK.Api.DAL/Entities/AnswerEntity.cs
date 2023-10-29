using AutoMapper;
using TaHooK.Common.Enums;

namespace TaHooK.Api.DAL.Entities;

public record AnswerEntity : EntityBase
{
    public required AnswerType Type { get; set; }
    public required string Text { get; set; }
    public Uri? Picture { get; set; }
    public required bool IsCorrect { get; set; }

    public required Guid QuestionId { get; set; }
    public required QuestionEntity Question { get; set; }


    public class AnswerEntityMapperProfile : Profile
    {
        public AnswerEntityMapperProfile()
        {
            CreateMap<AnswerEntity, AnswerEntity>();
        }
    }
}