using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Domain.Configurations;
using Store.Infrastructure.Configurations;
using Store.Infrastructure.Data;

namespace Store.CrossCutting.Extensions
{
    public static class DatabaseServiceExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection(Contants.DatabaseOptions));

            services.AddDbContext<StoreDbContext>(options =>
            {
                var dbOptions = configuration.GetSection(Contants.DatabaseOptions).Get<DatabaseOptions>();
                options.UseNpgsql(dbOptions?.ConnectionString);
            });

            return services;
        }
    }
}
