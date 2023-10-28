using Microsoft.AspNetCore.Mvc;
using TaHooK.Common.Models.Search;
using TaHooK.Api.BL.Facades;

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

    public IEnumerable<SearchListModel> GetSearch([FromQuery] SearchParams searchParams)
    {

        return _searchFacade.GetSearched(searchParams.Query,searchParams.Page,searchParams.PageSize);
    }

}

public class SearchParams
{
    [FromQuery(Name = "q")]
    public string Query { get; set; } = "";

    [FromQuery(Name = "p")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "size")]
    public int PageSize { get; set; } = 10;
}