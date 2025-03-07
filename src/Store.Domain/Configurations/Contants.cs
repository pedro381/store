namespace Store.Domain.Configurations
{
    public static class Contants
    {
        public static string CorrelationId => "X-Correlation-ID";
        public static string TokenSettingsSecretKey => "TokenSettings:SecretKey";
        public static string DatabaseOptions => "DatabaseOptions";
        public static string ApplicationJson => "application/json";
        public static string Health => "Health";
        public static string HealthCheck => "HealthCheck";
        public static string HealthUri => "/health";
        public static string DatabaseHealthCheck => "Database Health Check";
        public static string ExceptionToken => "Could not read token configurations.";
        public static string ApiName => "Store.Api";
        public static string Version => "v1";
        public static string Bearer => "Bearer";
        public static string DescriptionBearerHeader => "JWT Authorization Header - Utilizar 'Bearer <TOKEN>'.";
        public static string NameAuthorization => "Authorization";
        public static string BearerFormat => "JWT";
        public static string SwaggerEndpoint => "/swagger/v1/swagger.json";
        public static string ApiNameVersion => $"{ApiName} {Version}";
        public static string HealthyDatabaseQuery => "SELECT 1";
        public static string HealthyDatabase => "Database is operational.";
        public static string UnhealthyDatabase => "Database check failed.";
        public static string InvalidToken => "Invalid token";
    }
}
