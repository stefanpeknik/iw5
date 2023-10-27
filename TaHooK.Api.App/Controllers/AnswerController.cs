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
        return await _answerFacade.GetAllAsync();
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
        return await _answerFacade.CreateAsync(answer);        
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateAnswerById(AnswerDetailModel answer, Guid id)
    {
        if (answer.Id != id) return BadRequest("Answer IDs in URI and body don't match");
        
        return await _answerFacade.UpdateAsync(answer);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAnswer(Guid id)
    {
        await _answerFacade.DeleteAsync(id);
        return Ok();
    }
}