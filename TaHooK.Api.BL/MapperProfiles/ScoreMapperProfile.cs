using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Models.Score;

namespace TaHooK.Api.BL.MapperProfiles;

public class ScoreMapperProfile : Profile
{
    public ScoreMapperProfile()
    {
        CreateMap<ScoreEntity,ScoreDetailModel>();

        CreateMap<ScoreEntity, ScoreListModel>();
        
        CreateMap<ScoreDetailModel, ScoreEntity>();
    }
}