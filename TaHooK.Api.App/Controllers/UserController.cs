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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDetailModel>> GetUserById(Guid id)
    {
        var result = await _userFacade.GetByIdAsync(id);

        if (result == null) return NotFound($"User with Id = {id} was not found");

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<UserDetailModel>> CreateUser(UserDetailModel user)
    {
        return await _userFacade.SaveAsync(user);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDetailModel>> UpdateUserById(UserDetailModel user, Guid id)
    {
        if (user.Id != id) return BadRequest("User IDs in URI and body don't match");

        var toUpdate = await _userFacade.GetByIdAsync(id);

        if (toUpdate == null) return NotFound($"User with Id = {id} was not found");

        return await _userFacade.SaveAsync(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        await _userFacade.DeleteAsync(id);
        return Ok();
    }
}