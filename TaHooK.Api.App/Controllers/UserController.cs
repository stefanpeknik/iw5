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

    public UserController(UserFacade userFacade)
    {
        _userFacade = userFacade;
    }

    [HttpGet]
    [OpenApiOperation("GetUsers", "Returns a list of all users.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<UserListModel>), Description = "Successful operation.")]
    public async Task<IEnumerable<UserListModel>> GetUsers()
    {
        return await _userFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    [OpenApiOperation("GetUserById", "Returns a user based on the GUID on input.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(UserDetailModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel), Description = "User not found.")]
    public async Task<ActionResult<UserDetailModel>> GetUserById(Guid id)
    {
        var result = await _userFacade.GetByIdAsync(id);

        if (result == null) return NotFound(new ErrorModel { Error = $"User with Id = {id} was not found" });

        return result;
    }

    [HttpPost]
    [OpenApiOperation("CreateUser", "Creates a new user.")]
    [SwaggerResponse(HttpStatusCode.Accepted, typeof(IdModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel), Description = "Incorrect input model.")]
    public async Task<ActionResult<IdModel>> CreateUser(UserCreateUpdateModel user)
    {
        var result = await _userFacade.CreateAsync(user);
        return Accepted(result);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateUserById", "Updates an existing user.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IdModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel), Description = "Incorrect input model.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel),
        Description = "User with the given ID was not found.")]
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
    [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel), Description = "User with input ID was not found.")]
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