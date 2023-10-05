using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using TaHooK.Api.Common.Tests;
using TaHooK.Api.Common.Tests.Factories;
using TaHooK.Api.Common.Tests.Seeds;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class DALTestsBase : IAsyncLifetime
    {
        protected IDbContextFactory<TestingDbContext> DbContextFactory { get; }
        protected TestingDbContext DbContextInstance { get; }
        protected UnitOfWork.UnitOfWork UnitOfWork { get; }

        protected DALTestsBase(ITestOutputHelper output)
        {
            DbContextFactory = new DbContextTestingFactory(GetType().FullName!, true);
            DbContextInstance = DbContextFactory.CreateDbContext();

            var mapper = new Mock<IMapper>();
            UnitOfWork = new UnitOfWork.UnitOfWork(DbContextInstance, mapper.Object);
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
