using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswerController : ControllerBase
{
    private readonly AnswerFacade _answerFacade;

    public AnswerController(AnswerFacade answerFacade)
    {
        _answerFacade = answerFacade;
    }

    [HttpGet]
    [OpenApiOperation("GetAnswers", "Returns a list of all answers.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<AnswerListModel>), Description = "Successful operation.")]
    public async Task<IEnumerable<AnswerListModel>> GetAnswers()
    {
        return await _answerFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    [OpenApiOperation("GetAnswerById", "Returns an answer based on the GUID on input.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(AnswerDetailModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel), Description = "Answer not found.")]
    public async Task<ActionResult<AnswerDetailModel>> GetAnswerById(Guid id)
    {
        var result = await _answerFacade.GetByIdAsync(id);

        if (result == null) return NotFound(new ErrorModel { Error = $"Answer with Id = {id} was not found" });

        return result;
    }

    [HttpPost]
    [OpenApiOperation("CreateAnswer", "Creates a new answer.")]
    [SwaggerResponse(HttpStatusCode.Created, typeof(IdModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel), Description = "Incorrect input model.")]
    public async Task<ActionResult<IdModel>> CreateAnswer(AnswerCreateUpdateModel answer)
    {
        var result = await _answerFacade.CreateAsync(answer);
        return Created($"/api/answers/{result}", result);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateAnswerById", "Updates an existing answer.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IdModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel), Description = "Incorrect input model.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel),
        Description = "Answer with the given ID was not found.")]
    public async Task<ActionResult<IdModel>> UpdateAnswerById(AnswerCreateUpdateModel answer, Guid id)
    {
        try
        {
            var result = await _answerFacade.UpdateAsync(answer, id);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel { Error = $"Answer with ID = {id} doesn't exist" });
        }
    }

    [HttpDelete("{id:guid}")]
    [OpenApiOperation("DeleteAnswer", "Deletes an answer based on the input ID.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel), Description = "Answer with input ID was not found.")]
    public async Task<ActionResult> DeleteAnswer(Guid id)
    {
        try
        {
            await _answerFacade.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel { Error = $"Answer with ID = {id} doesn't exist" });
        }

        return Ok();
    }
}