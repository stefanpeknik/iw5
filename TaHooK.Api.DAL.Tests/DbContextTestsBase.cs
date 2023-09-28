using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TaHooK.Api.Common.Tests;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class DbContextTestsBase : IAsyncLifetime
    {
        protected IDbContextFactory<TaHooKDbContext> DbContextFactory { get; }
        protected TaHooKDbContext DbContextInstance { get; }

        protected DbContextTestsBase(ITestOutputHelper output)
        {
            DbContextFactory = new DbContextTestingFactory(GetType().FullName!, seedTestingData: true);
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
