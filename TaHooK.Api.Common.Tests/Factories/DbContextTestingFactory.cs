using TaHooK.Api.DAL;
using Microsoft.EntityFrameworkCore;

namespace TaHooK.Api.Common.Tests.Factories
{
    public class DbContextTestingFactory : IDbContextFactory<TaHooKDbContext>
    {
        private readonly string _databaseName;
        private readonly bool _seedTestingData;

        public DbContextTestingFactory(string databaseName, bool seedTestingData = false)
        {
            _databaseName = databaseName;
            _seedTestingData = seedTestingData;
        }

        public TaHooKDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaHooKDbContext>();
            optionsBuilder.UseSqlite($"Data Source={_databaseName};Cache=Shared");

            return new TestingDbContext(optionsBuilder.Options, seedTestingData: _seedTestingData);
        }
    }
}