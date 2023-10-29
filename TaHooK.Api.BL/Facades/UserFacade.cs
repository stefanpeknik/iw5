using AutoMapper;
using TaHooK.Api.BL.Facades.Interfaces;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.BL.Facades;

public class UserFacade: CrudFacadeBase<UserEntity, UserListModel, UserDetailModel, UserCreateUpdateModel>, IUserFacade
{
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
    }
    
    public override List<string> NavigationPathDetails => new()
    {
        $"{nameof(UserEntity.Scores)}"
    };
}