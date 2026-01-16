using HealthScan.Application.DTOs;
using HealthScan.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthScan.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/products")
            .WithTags("Products")
            .WithOpenApi();

        group.MapGet("/{barcode}", GetByBarcode)
            .WithName("GetProductByBarcode")
            .WithSummary("Get product by barcode")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .Produces<ProductIncompleteDto>(StatusCodes.Status206PartialContent)
            .Produces<ProductNotFoundDto>(StatusCodes.Status404NotFound);

        group.MapGet("/search", Search)
            .WithName("SearchProducts")
            .WithSummary("Search products by name or barcode")
            .Produces<ProductSearchResponseDto>(StatusCodes.Status200OK);

        group.MapPost("/{barcode}/contribute", Contribute)
            .WithName("ContributeProduct")
            .WithSummary("Contribute product data")
            .Produces<ContributionResponseDto>(StatusCodes.Status201Created);
    }

    private static async Task<IResult> GetByBarcode(
        string barcode,
        [FromServices] IProductService productService,
        CancellationToken cancellationToken)
    {
        var result = await productService.GetByBarcodeAsync(barcode, cancellationToken);

        if (!result.Found)
        {
            return Results.NotFound(result.NotFound);
        }

        if (!result.IsComplete)
        {
            return Results.Json(result.Incomplete, statusCode: StatusCodes.Status206PartialContent);
        }

        return Results.Ok(result.Product);
    }

    private static async Task<IResult> Search(
        [FromQuery] string q,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromServices] IProductService productService = null!,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Results.BadRequest(new { error = "Query parameter 'q' is required" });
        }

        pageSize = Math.Clamp(pageSize, 1, 100);
        page = Math.Max(1, page);

        var result = await productService.SearchAsync(q, page, pageSize, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> Contribute(
        string barcode,
        [FromBody] ContributionRequestDto request,
        [FromServices] IProductService productService,
        CancellationToken cancellationToken)
    {
        var result = await productService.ContributeAsync(barcode, request, cancellationToken);
        return Results.Created($"/api/v1/contributions/{result.ContributionId}", result);
    }
}
