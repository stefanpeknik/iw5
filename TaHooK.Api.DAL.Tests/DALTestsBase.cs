using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TaHooK.Api.Common.Tests;
using TaHooK.Api.Common.Tests.Factories;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class DALTestsBase : IAsyncLifetime
    {
        protected IDbContextFactory<TestingDbContext> DbContextFactory { get; }
        protected TestingDbContext DbContextInstance { get; }

        protected DALTestsBase(ITestOutputHelper output)
        {
            DbContextFactory = new DbContextTestingFactory(GetType().FullName!, true);
            DbContextInstance = DbContextFactory.CreateDbContext();
        }

        public async Task InitializeAsync()
        {
            await DbContextInstance.Database.EnsureDeletedAsync();
            await DbContextInstance.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await DbContextInstance.Database.EnsureDeletedAsync();
            await DbContextInstance.DisposeAsync();
        }
    }
}
