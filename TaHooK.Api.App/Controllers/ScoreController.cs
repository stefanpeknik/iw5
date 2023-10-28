using Microsoft.AspNetCore.Mvc;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Score;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Route("api/scores")]
public class ScoreController : ControllerBase
{
    private readonly ScoreFacade _scoreFacade;

    public ScoreController(ScoreFacade scoreFacade)
    {
        _scoreFacade = scoreFacade;
    }

    [HttpGet]
    public async Task<IEnumerable<ScoreListModel>> GetScores()
    {
        return await _scoreFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ScoreDetailModel>> GetScoreById(Guid id)
    {
        var result = await _scoreFacade.GetByIdAsync(id);

        if (result == null) return NotFound($"Score with Id = {id} was not found");

        return result;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateScore(ScoreDetailModel score)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _scoreFacade.CreateAsync(score);
        return Accepted(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateScoreById(ScoreDetailModel score, Guid id)
    {
        score.Id = id;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var result = await _scoreFacade.UpdateAsync(score);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Score with ID = {id} doesn't exist");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteScore(Guid id)
    {
        try
        {
            await _scoreFacade.DeleteAsync(id);    
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Score with ID = {id} doesn't exist");
        }
        
        return Ok(id);
    }
}