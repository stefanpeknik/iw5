using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.Common.Tests;
using TaHooK.Api.Common.Tests.Factories;
using TaHooK.Api.DAL.Entities;
using Xunit;
using Xunit.Abstractions;

namespace TaHooK.Api.DAL.Tests
{
    public class DALTestsBase : IAsyncLifetime
    {
        protected IDbContextFactory<TaHooKDbContext> DbContextFactory { get; }
        protected TaHooKDbContext DbContextInstance { get; }
        protected UnitOfWork.UnitOfWork UnitOfWork { get; }

        protected DALTestsBase(ITestOutputHelper output)
        {
            DbContextFactory = new DbContextTestingFactory(GetType().FullName!, true);
            DbContextInstance = DbContextFactory.CreateDbContext();

            // TODO: refactor this for better approach
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<ScoreEntity.ScoreEntityMapperProfile>();
                cfg.AddProfile<UserEntity.UserEntityMapperProfile>();
                cfg.AddProfile<QuestionEntity.QuestionEntityMapperProfile>();
                cfg.AddProfile<AnswerEntity.AnswerEntityMapperProfile>();
                cfg.AddProfile<QuizEntity.QuizEntityMapperProfile>();
            });
            var mapper = mapperConfig.CreateMapper();
            
            UnitOfWork = new UnitOfWork.UnitOfWork(DbContextInstance, mapper);
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
