using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
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
    [OpenApiOperation("GetScores", "Returns a list of all the scores.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<ScoreListModel>), Description = "Successful operation.")]
    public async Task<IEnumerable<ScoreListModel>> GetScores()
    {
        return await _scoreFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    [OpenApiOperation("GetScoreById", "Returns a score based on the GUID on input.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(ScoreDetailModel), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "Score not found.")]
    public async Task<ActionResult<ScoreDetailModel>> GetScoreById(Guid id)
    {
        var result = await _scoreFacade.GetByIdAsync(id);

        if (result == null) return NotFound($"Score with Id = {id} was not found");

        return result;
    }

    [HttpPost]
    [OpenApiOperation("CreateScore", "Creates a new score.")]
    [SwaggerResponse(HttpStatusCode.Created, typeof(Guid), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Incorrect input model.")]
    public async Task<ActionResult<Guid>> CreateScore(ScoreDetailModel score)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _scoreFacade.CreateAsync(score);
        return Created($"/api/scores/{result}", result);

    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateScoreById", "Updates an existing score.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(Guid), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Incorrect input model.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "Score with the given ID was not found.")]
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
    [OpenApiOperation("DeleteScore", "Deletes a score based on the input ID.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Successful operation.")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void), Description = "Score with input ID was not found.")]
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