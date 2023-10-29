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
    public async Task<ActionResult<Guid>> CreateQuestion(QuestionDetailModel question)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _questionFacade.CreateAsync(question);
        return Accepted(result);
    }

    [HttpPut("{id:guid}")]
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