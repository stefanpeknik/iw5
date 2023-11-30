using System.Net.Http.Json;
using TaHooK.Common.Models.Search;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class SearchTests : EndToEndTestsBase
{
    private async Task<HttpResponseMessage> GetSearch(string? query = null, int? page = null, int? pageSize = null)
    {
        var argsString = "";
        if (query != null) argsString += $"q={query}";
        if (page != null) argsString += $"&p={page}";
        if (pageSize != null) argsString += $"&size={pageSize}";

        return await Client.Value.GetAsync("/api/search" + "?" + argsString);
    }

    [Fact]
    public async Task Search_Returns_At_Least_One_Answer()
    {
        // Arrange
        var query = "banana";
        var page = 1;
        var pageSize = 10;

        // Act
        var response = await GetSearch(query, page, pageSize);

        // Assert
        response.EnsureSuccessStatusCode();
        var answers = await response.Content.ReadFromJsonAsync<SearchListModel>();
        Assert.NotNull(answers);
        Assert.NotEmpty(answers.Items);
    }

    [Fact]
    public async Task Search_Returns_Empty_When_No_True_Matches()
    {
        // Arrange

        var query = "lol random stuff here that should not match anything";
        var page = 1;
        var pageSize = 10;

        // Act
        var response = await GetSearch(query, page, pageSize);

        // Assert
        response.EnsureSuccessStatusCode();
        var answers = await response.Content.ReadFromJsonAsync<SearchListModel>();
        Assert.NotNull(answers);
        Assert.Empty(answers.Items);
    }

    [Fact]
    public async Task Search_Returns_Two_Answers_When_Page_Size_Is_Two()
    {
        // Arrange
        var page = 2;
        var pageSize = 2;

        // Act
        var response = await GetSearch(page: page, pageSize: pageSize);

        // Assert
        response.EnsureSuccessStatusCode();
        var foundOnPage = await response.Content.ReadFromJsonAsync<SearchListModel>();
        Assert.NotNull(foundOnPage);
        Assert.Equal(pageSize, foundOnPage.Items.Count());
    }
}