using Microsoft.AspNetCore.Mvc;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserFacade _userFacade;

    public UserController(UserFacade userFacade)
    {
        _userFacade = userFacade;
    }

    [HttpGet]
    public async Task<IEnumerable<UserListModel>> GetUsers()
    {
        return await _userFacade.GetAllAsync();
    }

    [HttpPost]
    public async Task<UserDetailModel> CreateUser(UserDetailModel user)
    {
        return await _userFacade.SaveAsync(user);
    }
}