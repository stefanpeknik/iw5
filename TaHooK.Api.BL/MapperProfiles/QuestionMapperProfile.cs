using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Question;

namespace TaHooK.Api.BL.MapperProfiles;

public class QuestionMapperProfile : Profile
{
    public QuestionMapperProfile()
    {
        CreateMap<QuestionEntity,QuestionDetailModel>()
            .MapMember(dst => dst.Answers, src => src.Answers);

        CreateMap<QuestionEntity, QuestionListModel>();

        CreateMap<QuestionDetailModel, QuestionEntity>()
            .Ignore(dst => dst.Answers);
    }
}