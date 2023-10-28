using Microsoft.AspNetCore.Mvc;
using TaHooK.Api.BL.Facades;
using TaHooK.Api.BL.Facades.Interfaces;
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
        return await _scoreFacade.CreateAsync(score);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateScoreById(ScoreDetailModel score, Guid id)
    {
        if (score.Id != id) return BadRequest("Score IDs in URI and body don't match");

        Guid result;
        try
        {
            result = await _scoreFacade.UpdateAsync(score);
        }
        catch (InvalidOperationException e)
        {
            return NotFound($"Score with ID = {id} doesn't exist");
        }

        return result;
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteScore(Guid id)
    {
        await _scoreFacade.DeleteAsync(id);
        return Ok();
    }
}