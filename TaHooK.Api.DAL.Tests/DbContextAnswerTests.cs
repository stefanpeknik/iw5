using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class DbContextAnswerTests : DbContextTestsBase
    {

        public DbContextAnswerTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GetCount_Answers()
        {
            //Act
            var answers = await DbContextInstance.Answers
                .CountAsync();

            //Assert
            Assert.Equal(1, answers);
        }
    }
}
