using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Store.CrossCutting.Helpers
{
    public static class DatabaseMigrator
    {
        public static void MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
