using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Enums;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Search;

namespace TaHooK.Api.BL.MapperProfiles;

public class QuestionMapperProfile : Profile
{
    public QuestionMapperProfile()
    {
        CreateMap<QuestionEntity,QuestionDetailModel>()
            .MapMember(dst => dst.Answers, src => src.Answers);

        CreateMap<QuestionEntity, QuestionListModel>();

        CreateMap<QuestionDetailModel, QuestionEntity>()
            .Ignore(dst => dst.Answers)
            .Ignore(dst => dst.Quiz);
        
        CreateMap<QuestionEntity, SearchListModel>()
            .MapMember(dst => dst.Name, src => src.Text)
            .MapMember(dst => dst.Type, src => SearchEntityType.Question);
    }
}