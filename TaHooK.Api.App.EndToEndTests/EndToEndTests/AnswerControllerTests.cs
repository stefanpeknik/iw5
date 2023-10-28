using System.Net.Http.Json;
using TaHooK.Common.Enums;
using TaHooK.Common.Models.Answer;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests.EndToEndTests;

public class AnswerControllerTests : EndToEndTestsBase
{

    [Fact]
    public async Task GetAllAnswers_Returns_At_Last_One_Answer()
    {
        // Act
        var response = await client.Value.GetAsync("/api/answers");
        response.EnsureSuccessStatusCode();
        var answers = await response.Content.ReadFromJsonAsync<ICollection<AnswerListModel>>();
        
        // Assert
        Assert.NotNull(answers);
        Assert.NotEmpty(answers);
    }
    

}
