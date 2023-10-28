using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
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
    [OpenApiOperation("GetUsers","Returns a list of all users.")]
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
    public async Task<ActionResult<Guid>> CreateUser(UserDetailModel user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _userFacade.CreateAsync(user);
        return Accepted(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateUserById(UserDetailModel user, Guid id)
    {
        user.Id = id;

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var result = await _userFacade.UpdateAsync(user);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound($"User with ID = {id} doesn't exist");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        try
        {
            await _userFacade.DeleteAsync(id);    
        }
        catch (InvalidOperationException)
        {
            return NotFound($"User with ID = {id} doesn't exist");
        }
        
        return Ok(id);
    }
}
