using Microsoft.EntityFrameworkCore;
using TaHooK.Api.DAL.Tests;
using Xunit;
using Xunit.Abstractions;

namespace Project.DAL.Tests
{
    public class DbContextAnswerTests : DbContextTestsBase
    {

        public DbContextAnswerTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GetCount_User()
        {
            //Act
            var users = await DbContextInstance.Answers
                .CountAsync();

            //Assert
            Assert.Equal(1, users);
        }
    }
}
