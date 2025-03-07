using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Store.CrossCutting.HealthChecks;
using Store.Domain.Configurations;
using System.Text.Json;

namespace Store.CrossCutting.Extensions
{
    public static class HealthCheckServiceExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddCheck<DatabaseHealthCheck>(Contants.DatabaseHealthCheck);

            return services;
        }

        public static void UseCustomHealthChecks(this WebApplication app)
        {
            app.MapGet(Contants.HealthUri, async (HealthCheckService healthCheckService) =>
            {
                var report = await healthCheckService.CheckHealthAsync();
                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        exception = e.Value.Exception?.Message,
                        duration = e.Value.Duration.TotalMilliseconds
                    })
                });

                return Results.Content(result, Contants.ApplicationJson);
            })
                .WithName(Contants.HealthCheck)
                .WithTags(Contants.Health)
                .WithOpenApi();
        }
    }
}
