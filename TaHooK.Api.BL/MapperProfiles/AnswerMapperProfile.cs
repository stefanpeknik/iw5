using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Enums;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Responses;
using TaHooK.Common.Models.Search;

namespace TaHooK.Api.BL.MapperProfiles;

public class AnswerMapperProfile : Profile
{
    public AnswerMapperProfile()
    {
        CreateMap<AnswerEntity, AnswerDetailModel>();

        CreateMap<AnswerEntity, AnswerListModel>();
        
        CreateMap<AnswerListModel, AnswerEntity>()
            .Ignore(dst => dst.Question);

        CreateMap<AnswerEntity, AnswerCreateUpdateModel>();

        CreateMap<AnswerCreateUpdateModel, AnswerEntity>()
            .Ignore(dst => dst.Id)
            .Ignore(dst => dst.Question);

        CreateMap<AnswerEntity, IdModel>();

        CreateMap<AnswerEntity, SearchListItemModel>()
            .MapMember(dst => dst.Id, src => src.Question!.QuizTemplateId)
            .MapMember(dst => dst.Name, src => src.Text)
            .MapMember(dst => dst.Type, src => SearchEntityType.QuizTemplate);
    }
}