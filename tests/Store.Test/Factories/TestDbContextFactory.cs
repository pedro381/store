using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Data;

namespace Store.Test.Factories
{
    public static class TestDbContextFactory
    {
        public static StoreDbContext Create()
        {
            var options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new StoreDbContext(options);
        }
    }
}
