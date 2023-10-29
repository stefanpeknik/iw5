using System.Net;
using System.Net.Http.Json;
using TaHooK.Api.App.Controllers;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Common.Models.Answer;
using TaHooK.Common.Models.Search;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class SearchTests : EndToEndTestsBase
{
    public async Task<HttpResponseMessage> GetSearch(SearchParams searchParams)
    {
        return await client.Value.GetAsync("/api/search" + "?" 
                                                         + $"q={searchParams.Query}&p={searchParams.Page}&size={searchParams.PageSize}");
    }

    [Fact]
    public async Task Search_Returns_At_Least_One_Answer()
    {
        // Arrange
        var searchParams = new SearchParams
        {
            Query = "banana",
            Page = 1,
            PageSize = 10
        };

        // Act
        var response = await GetSearch(searchParams);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var answers = await response.Content.ReadFromJsonAsync<ICollection<SearchListModel>>();
        Assert.NotNull(answers);
        Assert.NotEmpty(answers);
    }
    
    [Fact]
    public async Task Search_Returns_Empty_When_No_True_Matches()
    {
        // Arrange
        var searchParams = new SearchParams
        {
            Query = "lol random stuff here that should not match anything",
            Page = 1,
            PageSize = 10
        };

        // Act
        var response = await GetSearch(searchParams);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var answers = await response.Content.ReadFromJsonAsync<ICollection<SearchListModel>>();
        Assert.NotNull(answers);
        Assert.Empty(answers);
    }
    
    [Fact]
    public async Task Search_Returns_Two_Answers_When_Page_Size_Is_Two()
    {
        // Arrange
        const int pageSize = 2;
        var searchParams = new SearchParams
        {
            Query = "",
            Page = 2,
            PageSize = pageSize
        };

        // Act
        var response = await GetSearch(searchParams);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var foundOnPage = await response.Content.ReadFromJsonAsync<ICollection<SearchListModel>>();
        Assert.NotNull(foundOnPage);
        Assert.Equal(pageSize, foundOnPage.Count);
    }
    
