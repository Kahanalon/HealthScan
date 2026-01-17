using HealthScan.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthScan.Api.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/admin")
            .WithTags("Admin");

        group.MapPost("/import-israeli-products", ImportIsraeliProducts)
            .WithName("ImportIsraeliProducts")
            .WithSummary("Import Israeli products from Open Food Facts")
            .WithDescription("Fetches and imports Israeli products from Open Food Facts database. Duplicates are automatically skipped.")
            .Produces<ImportResult>(StatusCodes.Status200OK);

        group.MapGet("/stats", GetStats)
            .WithName("GetDatabaseStats")
            .WithSummary("Get database statistics")
            .Produces<DatabaseStats>(StatusCodes.Status200OK);
    }

    private static async Task<IResult> ImportIsraeliProducts(
        [FromQuery] int maxProducts = 1000,
        [FromServices] IImportService importService = null!,
        CancellationToken cancellationToken = default)
    {
        maxProducts = Math.Clamp(maxProducts, 1, 10000);
        var result = await importService.ImportIsraeliProductsAsync(maxProducts, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetStats(
        [FromServices] Domain.Interfaces.IProductRepository repository,
        [FromServices] Domain.Interfaces.IProductDataSource dataSource,
        CancellationToken cancellationToken)
    {
        var localCount = await repository.GetTotalCountAsync(cancellationToken);
        var remoteCount = await dataSource.GetIsraeliProductCountAsync(cancellationToken);

        return Results.Ok(new DatabaseStats
        {
            LocalProductCount = localCount,
            AvailableIsraeliProducts = remoteCount,
            CoveragePercentage = remoteCount > 0 ? Math.Round((double)localCount / remoteCount * 100, 2) : 0
        });
    }
}

public record DatabaseStats
{
    public int LocalProductCount { get; init; }
    public int AvailableIsraeliProducts { get; init; }
    public double CoveragePercentage { get; init; }
}
