using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Quiz;
using TaHooK.Common.Models.Responses;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Route("api/templates")]
public class QuizTemplateController : ControllerBase
{
    private readonly QuizTemplateFacade _quizFacade;

    public QuizTemplateController(QuizTemplateFacade quizFacade)
    {
        _quizFacade = quizFacade;
    }

    [HttpGet]
    [OpenApiOperation("GetQuizTemplates", "Returns a list of all quiz templates.")]
    public async Task<IEnumerable<QuizTemplateListModel>> GetQuizTemplates()
    {
        return await _quizFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    [OpenApiOperation("GetQuizTemplateById", "Returns a quiz template based on the GUID on input.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(QuizTemplateDetailModel))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult<QuizTemplateDetailModel>> GetQuizTemplateById(Guid id)
    {
        var result = await _quizFacade.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new ErrorModel { Error = $"Quiz template with Id = {id} was not found" });
        }

        return result;
    }

    [HttpPost]
    [OpenApiOperation("CreateQuizTemplate", "Creates a new quiz template.")]
    [SwaggerResponse(HttpStatusCode.Created, typeof(IdModel))]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel))]
    public async Task<ActionResult<IdModel>> CreateQuizTemplate(QuizTemplateCreateUpdateModel quiz)
    {
        var result = await _quizFacade.CreateAsync(quiz);
        return Created($"/api/templates/{result}", result);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateQuizTemplateById", "Updates an existing quiz template.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IdModel))]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult<IdModel>> UpdateQuizTemplateById(QuizTemplateCreateUpdateModel quiz, Guid id)
    {
        try
        {
            var result = await _quizFacade.UpdateAsync(quiz, id);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel { Error = $"Quiz with ID = {id} doesn't exist" });
        }
    }

    [HttpDelete("{id:guid}")]
    [OpenApiOperation("DeleteQuizTemplate", "Deletes a quiz template based on the input ID.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(void))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult> DeleteQuizTemplate(Guid id)
    {
        try
        {
            await _quizFacade.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel { Error = $"Quiz template with ID = {id} doesn't exist" });
        }

        return Ok();
    }
}