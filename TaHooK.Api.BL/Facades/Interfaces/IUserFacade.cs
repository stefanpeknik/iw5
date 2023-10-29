using TaHooK.Api.DAL.Entities;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades.Interfaces;

public interface IUserFacade: ICrudFacade<UserEntity, UserListModel, UserDetailModel, UserCreateUpdateModel>
{
    
}
