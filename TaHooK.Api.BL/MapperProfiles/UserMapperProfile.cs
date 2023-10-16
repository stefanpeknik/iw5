using AutoMapper;
using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Extensions;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.MapperProfiles;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserEntity,UserDetailModel>()
            .MapMember(dst => dst.Scores,src => src.Scores);
        
        CreateMap<UserEntity, UserListModel>();
        
        CreateMap<UserDetailModel, UserEntity>()
            .Ignore(dst => dst.Scores);
    }
}