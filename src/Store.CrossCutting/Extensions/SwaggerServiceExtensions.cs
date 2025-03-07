using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Store.Domain.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Store.CrossCutting.Extensions
{
    public static class BodyLoggingMiddleware
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Contants.Version, new OpenApiInfo { Title = Contants.ApiName, Version = Contants.Version });
                c.AddSecurityDefinition(Contants.Bearer, new OpenApiSecurityScheme
                {
                    Description = Contants.DescriptionBearerHeader,
                    Name = Contants.NameAuthorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = Contants.Bearer,
                    BearerFormat = Contants.BearerFormat,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = Contants.Bearer
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                c.OperationFilter<AddCorrelationIdHeaderParameter>();
            });


            return services;
        }

        public static void Swagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(Contants.SwaggerEndpoint, Contants.ApiNameVersion);
                });
            }

        }
    }
    public class AddCorrelationIdHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = [];

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = Contants.CorrelationId,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString(Guid.NewGuid().ToString())
                }
            });
        }
    }
}
