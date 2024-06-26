﻿using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Responses;
using TaHooK.Common.Models.Score;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Authorize]
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
    public async Task<IEnumerable<ScoreListModel>> GetScores()
    {
        return await _scoreFacade.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    [OpenApiOperation("GetScoreById", "Returns a score based on the GUID on input.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(ScoreDetailModel))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult<ScoreDetailModel>> GetScoreById(Guid id)
    {
        var result = await _scoreFacade.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new ErrorModel { Error = $"Score with Id = {id} was not found" });
        }

        return result;
    }

    [HttpPost]
    [OpenApiOperation("CreateScore", "Creates a new score.")]
    [SwaggerResponse(HttpStatusCode.Created, typeof(IdModel))]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel))]
    public async Task<ActionResult<IdModel>> CreateScore(ScoreCreateUpdateModel score)
    {
        var result = await _scoreFacade.CreateAsync(score);
        return Created($"/api/scores/{result}", result);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("UpdateScoreById", "Updates an existing score.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(IdModel))]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestModel))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult<IdModel>> UpdateScoreById(ScoreCreateUpdateModel score, Guid id)
    {
        try
        {
            var result = await _scoreFacade.UpdateAsync(score, id);
            return result;
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel { Error = $"Score with ID = {id} doesn't exist" });
        }
    }

    [HttpDelete("{id:guid}")]
    [OpenApiOperation("DeleteScore", "Deletes a score based on the input ID.")]
    [SwaggerResponse(HttpStatusCode.OK, typeof(void))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorModel))]
    public async Task<ActionResult> DeleteScore(Guid id)
    {
        try
        {
            await _scoreFacade.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ErrorModel { Error = $"Score with ID = {id} doesn't exist" });
        }

        return Ok();
    }
}