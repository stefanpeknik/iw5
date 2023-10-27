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
        return await _quizFacade.CreateAsync(quiz);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateQuizById(QuizDetailModel quiz, Guid id)
    {
        if (quiz.Id != id) return BadRequest("Quiz IDs in URI and body don't match");

        return await _quizFacade.UpdateAsync(quiz);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteQuiz(Guid id)
    {
        await _quizFacade.DeleteAsync(id);
        return Ok();
    }
}