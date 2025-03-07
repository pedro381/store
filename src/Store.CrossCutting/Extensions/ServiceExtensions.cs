using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Profiles;
using Store.Application.Queries;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Store.Domain.Configurations;
using Store.CrossCutting.Authentications;

namespace Store.CrossCutting.Extensions
{
    public static class ExceptionHandlingMiddleware
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();

            services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Contants.ApiName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter();
            });

            services.AddDatabaseConfiguration(configuration)
                    .AddRepositoryServices()
                    .AddSwaggerConfiguration()
                    .AddCustomHealthChecks()
                    .AddAuthenticationManager(configuration);

            var tokenSettings = configuration.GetSection(
                typeof(TokenSettings).Name).Get<TokenSettings>()
                ?? throw new Exception(Contants.ExceptionToken);
            services.AddSingleton(x => tokenSettings);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetOrdersQueryHandler).Assembly));

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddControllers();
        }
    }
}
