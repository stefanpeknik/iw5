using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TaHooK.Api.BL.Facades;
using TaHooK.Common.Models.Search;

namespace TaHooK.Api.App.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly SearchFacade _searchFacade;

    public SearchController(SearchFacade searchFacade)
    {
        _searchFacade = searchFacade;
    }

    [HttpGet]
    [OpenApiOperation("GetSearch", "Returns a list of found entities based on the searched query.")]
    public SearchListModel GetSearch([FromQuery] SearchParams searchParams)
    {
        return _searchFacade.GetSearched(searchParams.Query, searchParams.Page, searchParams.PageSize);
    }
}

public class SearchParams
{
    [FromQuery(Name = "q")] public string Query { get; set; } = "";

    [FromQuery(Name = "p")] public int Page { get; set; } = 1;

    [FromQuery(Name = "size")] public int PageSize { get; set; } = 10;
}