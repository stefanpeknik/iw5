using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Question;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Route("api/questions")]
public class QuestionController : ControllerBase
{
    private readonly QuestionFacade _questionFacade;

    public QuestionController(QuestionFacade questionFacade)
    {
        _questionFacade = questionFacade;
    }

    [HttpGet]
    [OpenApiOperation("GetQuestions", "Returns a list of all questions.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<QuestionListModel>), Description = "Successful operation.")]
    public async Task<IEnumerable<QuestionListModel>> GetQuestions()
    {
        return await _questionFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    [OpenApiOperation("GetQuestionById", "Returns a question based on the GUID on input.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(QuestionDetailModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "User not found.")]
    public async Task<ActionResult<QuestionDetailModel>> GetQuestionById(Guid id)
    {
        var result = await _questionFacade.GetByIdAsync(id);

        if (result == null) return NotFound($"Question with Id = {id} was not found");

        return result;
    }

    [HttpPost]
    [OpenApiOperation("CreateQuestion", "Creates a new question.")]
    [SwaggerResponse(HttpStatusCode.Created, typeof(Guid), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Incorrect input model.")]
    public async Task<ActionResult<Guid>> CreateQuestion(QuestionDetailModel question)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _questionFacade.CreateAsync(question);
        return Created($"/api/questions/{result}", result);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateQuestionById", "Updates an existing question.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(Guid), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Incorrect input model.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "Question with the given ID was not found.")]
    public async Task<ActionResult<Guid>> UpdateQuestionById(QuestionDetailModel question, Guid id)
    {
        question.Id = id;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
           var result = await _questionFacade.UpdateAsync(question);
           return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Question with ID = {id} doesn't exist");
        }
    }

    [HttpDelete("{id:guid}")]
    [OpenApiOperation("DeleteQuestion", "Deletes a question based on the input ID.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "Question with input ID was not found.")]
    public async Task<ActionResult> DeleteQuestion(Guid id)
    {
        try
        {
            await _questionFacade.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Question with ID = {id} doesn't exist");
        }

        return Ok(id);
    }
}