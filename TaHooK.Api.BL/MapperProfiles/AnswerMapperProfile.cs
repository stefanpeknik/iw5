using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Models.Answer;

namespace TaHooK.Api.BL.MapperProfiles;

public class AnswerMapperProfile : Profile
{
    public AnswerMapperProfile()
    {
        CreateMap<AnswerEntity,AnswerDetailModel>();
        
        CreateMap<AnswerEntity, AnswerListModel>();

        CreateMap<AnswerDetailModel, AnswerEntity>();
    }
}