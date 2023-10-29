using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Enums;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.Responses;
using TaHooK.Common.Models.Search;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.MapperProfiles;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserEntity, UserDetailModel>()
            .MapMember(dst => dst.Scores, src => src.Scores);

        CreateMap<UserEntity, UserListModel>();

        CreateMap<UserEntity, UserCreateUpdateModel>();

        CreateMap<UserCreateUpdateModel, UserEntity>()
            .Ignore(dst => dst.Id)
            .Ignore(dst => dst.Scores);

        CreateMap<UserEntity, IdModel>();

        CreateMap<UserEntity, SearchListModel>()
            .MapMember(dst => dst.Name, src => src.Name)
            .MapMember(dst => dst.Type, src => SearchEntityType.User);
    }
}