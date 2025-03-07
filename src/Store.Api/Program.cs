using Store.CrossCutting.Extensions;
using Store.CrossCutting.Middlewares;
using Store.CrossCutting.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase();
app.Swagger();
app.UseCustomHealthChecks();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.UseExceptionHandlingMiddleware(); 
app.UseCorrelationIdMiddleware();   
app.UseBodyLoggingMiddleware();

app.Run();
