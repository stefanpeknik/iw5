using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Answer;

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
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "Answer not found.")]
    public async Task<ActionResult<AnswerDetailModel>> GetAnswerById(Guid id)
    {
        var result = await _answerFacade.GetByIdAsync(id);

        if (result == null) return NotFound($"Answer with Id = {id} was not found");

        return result;
    }

    [HttpPost]
    [OpenApiOperation("CreateAnswer", "Creates a new answer.")]
    [SwaggerResponse(HttpStatusCode.Created, typeof(Guid), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Incorrect input model.")]
    public async Task<ActionResult<Guid>> CreateAnswer(AnswerDetailModel answer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _answerFacade.CreateAsync(answer);
        return Created($"/api/answers/{result}", result);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateAnswerById", "Updates an existing answer.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(Guid), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Incorrect input model.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "Answer with the given ID was not found.")]
    public async Task<ActionResult<Guid>> UpdateAnswerById(AnswerDetailModel answer, Guid id)
    {
        answer.Id = id;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var result = await _answerFacade.UpdateAsync(answer);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Answer with ID = {id} doesn't exist");
        }
    }

    [HttpDelete("{id:guid}")]
    [OpenApiOperation("DeleteAnswer", "Deletes an answer based on the input ID.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "Answer with input ID was not found.")]
    public async Task<ActionResult> DeleteAnswer(Guid id)
    {
        try
        {
            await _answerFacade.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Answer with ID = {id} doesn't exist");
        }

        return Ok(id);
    }
}