using Microsoft.Extensions.Diagnostics.HealthChecks;
using Store.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Configurations;

namespace Store.CrossCutting.HealthChecks
{
    public class DatabaseHealthCheck(StoreDbContext dbContext) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await dbContext.Database.ExecuteSqlRawAsync(Contants.HealthyDatabaseQuery, cancellationToken);
                return HealthCheckResult.Healthy(Contants.HealthyDatabase);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(Contants.UnhealthyDatabase, ex);
            }
        }
    }
}
