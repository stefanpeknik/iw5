using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Enums;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Responses;
using TaHooK.Common.Models.Search;

namespace TaHooK.Api.BL.MapperProfiles;

public class AnswerMapperProfile : Profile
{
    public AnswerMapperProfile()
    {
        CreateMap<AnswerEntity,AnswerDetailModel>();
        
        CreateMap<AnswerEntity, AnswerListModel>();
        
        CreateMap<AnswerEntity, AnswerCreateUpdateModel>();

        CreateMap<AnswerCreateUpdateModel, AnswerEntity>()
            .Ignore(dst => dst.Id)
            .Ignore(dst => dst.Question);
        
        CreateMap<AnswerEntity, IdModel>();
        
        CreateMap<AnswerEntity, SearchListModel>()
            .MapMember(dst => dst.Name, src => src.Text)
            .MapMember(dst => dst.Type, src => SearchEntityType.Answer);
    }
}