using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Responses;
using TaHooK.Common.Models.User;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserFacade _userFacade;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(UserFacade userFacade, IHttpContextAccessor httpContextAccessor)
    {
        _userFacade = userFacade;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [OpenApiOperation("GetUsers", "Returns a list of all users.")]
    public async Task<IEnumerable<UserListModel>> GetUsers()
    {
        return await _userFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    [OpenApiOperation("GetUserById", "Returns a user based on the GUID on input.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(UserDetailModel))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult<UserDetailModel>> GetUserById(Guid id)
    {
        var result = await _userFacade.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new ErrorModel { Error = $"User with Id = {id} was not found" });
        }

        return result;
    }

    [HttpPost]
    [OpenApiOperation("Create Or Update User", "Creates or updates user with JWT ID.")]
    [SwaggerResponse(HttpStatusCode.Created, typeof(IdModel))]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel))]
    public async Task<ActionResult<IdModel>> CreateUser(UserCreateUpdateModel user)
    {
        Guid id;
        try
        {
            var idClaim = _httpContextAccessor.HttpContext!.User.Claims.First(claim => claim.Type.Equals("Id"));
            id = Guid.Parse(idClaim.Value);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new ErrorModel {Error = "JWT ID not found."});
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(new ErrorModel {Error = "JWT ID not found."});
        }
        catch (FormatException e)
        {
            return BadRequest(new ErrorModel {Error = "JWT ID has invalid format."});
        }

        var result = await _userFacade.CreateOrUpdateAsync(user, id);
        return Created($"/api/users/{result}",result);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateUserById", "Updates an existing user.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IdModel))]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult<IdModel>> UpdateUserById(UserCreateUpdateModel user, Guid id)
    {
        try
        {
            var result = await _userFacade.UpdateAsync(user, id);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel { Error = $"User with ID = {id} doesn't exist" });
        }
    }
    
    [HttpDelete("{id:guid}")]
    [OpenApiOperation("DeleteUser", "Deletes a user based on the input ID.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(void))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        try
        {
            await _userFacade.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel { Error = $"User with ID = {id} doesn't exist" });
        }

        return Ok();
    }
}