using Microsoft.Extensions.DependencyInjection;
using Store.Infrastructure.Interfaces;
using Store.Infrastructure.Repositories;

namespace Store.CrossCutting.Extensions
{
    public static class UserLoggingMiddleware
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDiscountConfigurationRepository, DiscountConfigurationRepository>();

            return services;
        }
    }
}
