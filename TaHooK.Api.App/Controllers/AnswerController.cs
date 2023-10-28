using Microsoft.AspNetCore.Mvc;
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
    public async Task<IEnumerable<AnswerListModel>> GetAnswers()
    {
        var result = await _answerFacade.GetAllAsync();
        return result;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AnswerDetailModel>> GetAnswerById(Guid id)
    {
        var result = await _answerFacade.GetByIdAsync(id);

        if (result == null) return NotFound($"Answer with Id = {id} was not found");

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAnswer(AnswerDetailModel answer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _answerFacade.CreateAsync(answer);
        return Accepted(result);        
    }

    [HttpPut("{id:guid}")]
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