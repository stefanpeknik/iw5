using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class DbContextQuestionTests : DbContextTestsBase
    {

        public DbContextQuestionTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GetCount_Questions()
        {
            //Act
            var questions = await DbContextInstance.Questions
                .CountAsync();

            //Assert
            Assert.Equal(1, questions);
        }
    }
}