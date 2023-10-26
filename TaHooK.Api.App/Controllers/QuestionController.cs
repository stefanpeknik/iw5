using Microsoft.AspNetCore.Mvc;
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
    public async Task<IEnumerable<QuestionListModel>> GetQuestions()
    {
        return await _questionFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<QuestionDetailModel>> GetQuestionById(Guid id)
    {
        var result = await _questionFacade.GetByIdAsync(id);

        if (result == null) return NotFound($"Question with Id = {id} was not found");

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<QuestionDetailModel>> CreateQuestion(QuestionDetailModel question)
    {
        return await _questionFacade.SaveAsync(question);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<QuestionDetailModel>> UpdateQuestionById(QuestionDetailModel question, Guid id)
    {
        if (question.Id != id) return BadRequest("Question IDs in URI and body don't match");

        var toUpdate = await _questionFacade.GetByIdAsync(id);

        if (toUpdate == null) return NotFound($"Question with Id = {id} was not found");

        return await _questionFacade.SaveAsync(question);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteQuestion(Guid id)
    {
        await _questionFacade.DeleteAsync(id);
        return Ok();
    }
}