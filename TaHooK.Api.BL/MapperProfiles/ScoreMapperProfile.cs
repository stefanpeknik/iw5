using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Responses;
using TaHooK.Common.Models.Score;

namespace TaHooK.Api.BL.MapperProfiles;

public class ScoreMapperProfile : Profile
{
    public ScoreMapperProfile()
    {
        CreateMap<ScoreEntity, ScoreDetailModel>();

        CreateMap<ScoreEntity, ScoreListModel>()
            .MapMember(dst => dst.UserName, src => src.User!.Name);

        CreateMap<ScoreEntity, ScoreCreateUpdateModel>();

        CreateMap<ScoreCreateUpdateModel, ScoreEntity>()
            .Ignore(dst => dst.Id)
            .Ignore(dst => dst.User)
            .Ignore(dst => dst.Quiz);

        CreateMap<ScoreEntity, IdModel>();
    }
}