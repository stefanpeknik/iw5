using TaHooK.Api.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaHooK.Api.DAL.Factories;

namespace TaHooK.Api.Common.Tests.Factories
{
    public class DbContextTestingFactory : IDbContextFactory<TestingDbContext>
    {
        private readonly string _databaseName;
        private readonly bool _seedTestingData;

        public DbContextTestingFactory(string databaseName, bool seedTestingData = false)
        {
            _databaseName = databaseName;
            _seedTestingData = seedTestingData;
        }

        public TestingDbContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<TaHooKDbContextFactory>(optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TaHooKDbContext>();
            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("TestConnection") 
                + $"Database={_databaseName};");

            return new TestingDbContext(optionsBuilder.Options, seedTestingData: _seedTestingData);
        }
    }
}