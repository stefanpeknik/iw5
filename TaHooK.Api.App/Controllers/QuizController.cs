using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Route("api/quizzes")]
public class QuizController : ControllerBase
{
    private readonly QuizFacade _quizFacade;

    public QuizController(QuizFacade quizFacade)
    {
        _quizFacade = quizFacade;
    }

    [HttpGet]
    [OpenApiOperation("GetQuizzes", "Returns a list of all quizzes.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<QuizListModel>), Description = "Successful operation.")]
    public async Task<IEnumerable<QuizListModel>> GetQuizzes()
    {
        return await _quizFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    [OpenApiOperation("GetQuizById", "Returns a quiz based on the GUID on input.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(QuizDetailModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel), Description = "Quiz not found.")]
    public async Task<ActionResult<QuizDetailModel>> GetQuizById(Guid id)
    {
        var result = await _quizFacade.GetByIdAsync(id);

        if (result == null) return NotFound(new ErrorModel{ Error = $"Quiz with Id = {id} was not found" });

        return result;
    }

    [HttpPost]
    [OpenApiOperation("CreateQuiz", "Creates a new quiz.")]
    [SwaggerResponse(HttpStatusCode.Created, typeof(IdModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel), Description = "Incorrect input model.")]
    public async Task<ActionResult<IdModel>> CreateQuiz(QuizCreateUpdateModel quiz)
    {
        var result = await _quizFacade.CreateAsync(quiz);
        return Created($"/api/quizzes/{result}", result);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateQuizById", "Updates an existing quiz.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IdModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel), Description = "Incorrect input model.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel), Description = "Quiz with the given ID was not found.")]
    public async Task<ActionResult<IdModel>> UpdateQuizById(QuizCreateUpdateModel quiz, Guid id)
    {
        try
        {
            var result = await _quizFacade.UpdateAsync(quiz, id);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel{ Error = $"Quiz with ID = {id} doesn't exist"});
        }
    }

    [HttpDelete("{id:guid}")]
    [OpenApiOperation("DeleteQuiz", "Deletes a quiz based on the input ID.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel), Description = "Quiz with input ID was not found.")]
    public async Task<ActionResult> DeleteQuiz(Guid id)
    {
        try
        {
            await _quizFacade.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel{ Error = $"Quiz with ID = {id} doesn't exist"});
        }    
        
        return Ok();
    }
}