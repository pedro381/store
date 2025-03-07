using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Store.Domain.Configurations;
using System.Text;

namespace Store.CrossCutting.Authentications
{
    public static class AuthenticationManager
    {
        public static IServiceCollection AddAuthenticationManager(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration[Contants.TokenSettingsSecretKey] ?? string.Empty;
            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}
