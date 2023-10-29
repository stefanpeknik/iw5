using Microsoft.AspNetCore.Mvc;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Quiz;

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
    public async Task<IEnumerable<QuizListModel>> GetQuizzes()
    {
        return await _quizFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<QuizDetailModel>> GetQuizById(Guid id)
    {
        var result = await _quizFacade.GetByIdAsync(id);

        if (result == null) return NotFound($"Quiz with Id = {id} was not found");

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateQuiz(QuizDetailModel quiz)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _quizFacade.CreateAsync(quiz);
        return Accepted(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateQuizById(QuizDetailModel quiz, Guid id)
    {
        quiz.Id = id;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var result = await _quizFacade.UpdateAsync(quiz);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Quiz with ID = {id} doesn't exist");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteQuiz(Guid id)
    {
        try
        {
            await _quizFacade.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Quiz with ID = {id} doesn't exist");
        }    
        
        return Ok(id);
    }
}