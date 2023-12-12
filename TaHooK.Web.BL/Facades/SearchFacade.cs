using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaHooK.Common.Models;
using Microsoft.Extensions.Options;
using TaHooK.Common.Enums;
using TaHooK.Common.Models.Question;
using TaHooK.Common.Models.Search;
using TaHooK.Common.Models.Responses;


namespace TaHooK.Web.BL.Facades;

public class SearchFacade : IWebAppFacade
{
    private readonly ISearchApiClient _apiClient;
    
    public SearchFacade(ISearchApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    public async Task<SearchListModel> SearchAsync(string query, int page, int size)
    {
        return await _apiClient.SearchAsync(query, page, size);
    }
}
