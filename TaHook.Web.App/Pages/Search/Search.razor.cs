﻿using Microsoft.AspNetCore.Components;
using TaHooK.Common.Enums;
using TaHooK.Common.Models.Search;
using TaHooK.Web.BL.Facades;

namespace TaHook.Web.App.Pages.Search;

public partial class Search
{
    [Inject] private SearchFacade? SearchFacade { get; set; }
    [Inject] private NavigationManager? Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await SearchAsync();
    }
    
    private ICollection<SearchListItemModel> _answers = new List<SearchListItemModel>();
    private ICollection<SearchListItemModel> _questions = new List<SearchListItemModel>();
    private ICollection<SearchListItemModel> _users = new List<SearchListItemModel>();
    
    private string _query = "";
    private int _page = 1;
    private int _size = 10;
    
    
    private async Task SearchAsync()
    {
        if (string.IsNullOrEmpty(_query)) return; // cant search for empty query
        if (SearchFacade == null) return; // cant search without facade
        var searched = await SearchFacade.SearchAsync(_query, _page, _size);
        await SortSearchResultsAsync(searched);  
        await InvokeAsync(StateHasChanged);
    }
    
    protected void OnShowQuizTemplateDetail(Guid quizId)
    {
        Navigation!.NavigateTo($"/quiz/{quizId}");
    }
    
    private async Task SortSearchResultsAsync(SearchListModel searchResults)
    {
        _answers = new List<SearchListItemModel>();
        _questions = new List<SearchListItemModel>();
        _users = new List<SearchListItemModel>();
        
        foreach (var item in searchResults.Items)
        {
            switch (item.Type)
            {
                case SearchEntityType.Answer:
                    _answers.Add(item);
                    break;
                case SearchEntityType.Question:
                    _questions.Add(item);
                    break;
                case SearchEntityType.User:
                    _users.Add(item);
                    break;
            }
        }
    }
    
    private async Task UpdateQuery(ChangeEventArgs e)
    {
        _query = e.Value!.ToString()!;
        await SearchAsync();
    }
    
    private async Task ChangePage(int delta)
    {
        _page += delta;
        await SearchAsync();
    }
}