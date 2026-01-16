namespace HealthScan.Api.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok(new
        {
            status = "healthy",
            version = "1.0.0",
            timestamp = DateTime.UtcNow
        }))
        .WithName("HealthCheck")
        .WithTags("Health")
        .WithSummary("Health check endpoint");

        app.MapGet("/api/v1/health", () => Results.Ok(new
        {
            status = "healthy",
            version = "1.0.0",
            timestamp = DateTime.UtcNow
        }))
        .WithName("ApiHealthCheck")
        .WithTags("Health")
        .WithSummary("API health check endpoint");
    }
}
