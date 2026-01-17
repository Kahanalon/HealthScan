using HealthScan.Application.Services;
using HealthScan.Domain.Interfaces;
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

        var offGroup = app.MapGroup("/api/v1/admin/off")
            .WithTags("OpenFoodFacts Data");

        offGroup.MapPost("/import", ImportOffProducts)
            .WithName("ImportOffProducts")
            .WithSummary("Import full OFF data for Israeli products")
            .WithDescription("Imports complete OpenFoodFacts data (nutrition, scores, ingredients, images, environment) into dedicated OFF tables.")
            .Produces<OffImportResult>(StatusCodes.Status200OK);

        offGroup.MapPost("/sync/{barcode}", SyncOffProduct)
            .WithName("SyncOffProduct")
            .WithSummary("Sync single product from OFF API")
            .WithDescription("Fetches latest data for a single product from OpenFoodFacts API and updates local OFF tables.")
            .Produces<OffProductDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        offGroup.MapGet("/stats", GetOffStats)
            .WithName("GetOffStats")
            .WithSummary("Get OFF data statistics")
            .WithDescription("Returns statistics about the OFF data including score distributions and coverage.")
            .Produces<OffStats>(StatusCodes.Status200OK);

        offGroup.MapGet("/{barcode}", GetOffProduct)
            .WithName("GetOffProduct")
            .WithSummary("Get OFF product by barcode")
            .WithDescription("Retrieves complete OFF data for a product including all related tables.")
            .Produces<OffProductDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        offGroup.MapPost("/pull/{barcode}", PullToProduct)
            .WithName("PullOffToProduct")
            .WithSummary("Pull OFF data into main Product table")
            .WithDescription("Copies relevant OFF data into the main products table for the specified barcode.")
            .Produces<Domain.Entities.Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        offGroup.MapPost("/pull-all", PullAllToProducts)
            .WithName("PullAllOffToProducts")
            .WithSummary("Pull all unlinked OFF data into Products")
            .WithDescription("Creates Product entries for all OFF products that don't have a corresponding Product record.")
            .Produces<PullAllResult>(StatusCodes.Status200OK);
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

    private static async Task<IResult> ImportOffProducts(
        [FromQuery] int maxProducts = 10000,
        [FromServices] IOffImportService importService = null!,
        CancellationToken cancellationToken = default)
    {
        maxProducts = Math.Clamp(maxProducts, 1, 50000);
        var result = await importService.ImportIsraeliProductsAsync(maxProducts, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> SyncOffProduct(
        string barcode,
        [FromServices] IOffSyncService syncService = null!,
        CancellationToken cancellationToken = default)
    {
        var product = await syncService.SyncProductAsync(barcode, cancellationToken);

        if (product == null)
        {
            return Results.NotFound(new { message = $"Product not found in OpenFoodFacts: {barcode}" });
        }

        return Results.Ok(MapToDto(product));
    }

    private static async Task<IResult> GetOffStats(
        [FromServices] IOffSyncService syncService = null!,
        CancellationToken cancellationToken = default)
    {
        var stats = await syncService.GetStatsAsync(cancellationToken);
        return Results.Ok(stats);
    }

    private static async Task<IResult> GetOffProduct(
        string barcode,
        [FromServices] IOffSyncService syncService = null!,
        CancellationToken cancellationToken = default)
    {
        var product = await syncService.GetProductAsync(barcode, cancellationToken);

        if (product == null)
        {
            return Results.NotFound(new { message = $"OFF product not found locally: {barcode}" });
        }

        return Results.Ok(MapToDto(product));
    }

    private static async Task<IResult> PullToProduct(
        string barcode,
        [FromServices] IOffPullService pullService = null!,
        CancellationToken cancellationToken = default)
    {
        var product = await pullService.PullToProductAsync(barcode, cancellationToken);

        if (product == null)
        {
            return Results.NotFound(new { message = $"OFF product not found: {barcode}" });
        }

        return Results.Ok(product);
    }

    private static async Task<IResult> PullAllToProducts(
        [FromServices] IOffPullService pullService = null!,
        CancellationToken cancellationToken = default)
    {
        var count = await pullService.PullAllUnlinkedAsync(cancellationToken);
        return Results.Ok(new PullAllResult
        {
            PulledCount = count,
            Message = $"Successfully created {count} products from OFF data"
        });
    }

    private static OffProductDto MapToDto(Domain.Entities.OpenFoodFacts.OffProduct product)
    {
        return new OffProductDto
        {
            Barcode = product.Barcode,
            ProductName = product.ProductName,
            ProductNameHe = product.ProductNameHe,
            ProductNameEn = product.ProductNameEn,
            Brands = product.Brands,
            Quantity = product.Quantity,
            Categories = product.Categories,
            Completeness = product.Completeness,
            LastSyncedAt = product.LastSyncedAt,
            Nutrition = product.Nutrition != null ? new OffNutritionDto
            {
                EnergyKcal100g = product.Nutrition.EnergyKcal100g,
                Fat100g = product.Nutrition.Fat100g,
                SaturatedFat100g = product.Nutrition.SaturatedFat100g,
                Carbohydrates100g = product.Nutrition.Carbohydrates100g,
                Sugars100g = product.Nutrition.Sugars100g,
                Fiber100g = product.Nutrition.Fiber100g,
                Proteins100g = product.Nutrition.Proteins100g,
                Sodium100g = product.Nutrition.Sodium100g,
                Salt100g = product.Nutrition.Salt100g,
                NutritionGradeFr = product.Nutrition.NutritionGradeFr
            } : null,
            Ingredients = product.Ingredients != null ? new OffIngredientsDto
            {
                IngredientsText = product.Ingredients.IngredientsTextEn ?? product.Ingredients.IngredientsText,
                IngredientsCount = product.Ingredients.IngredientsCount,
                AllergensTags = product.Ingredients.AllergensTags,
                AdditivesTags = product.Ingredients.AdditivesTags,
                NovaGroup = product.Ingredients.NovaGroup,
                IsVegan = product.Ingredients.IsVegan,
                IsVegetarian = product.Ingredients.IsVegetarian,
                IsPalmOilFree = product.Ingredients.IsPalmOilFree
            } : null,
            Scores = product.Scores != null ? new OffScoresDto
            {
                NutriScoreGrade = product.Scores.NutriScoreGrade,
                NutriScoreScore = product.Scores.NutriScoreScore,
                EcoScoreGrade = product.Scores.EcoScoreGrade,
                EcoScoreScore = product.Scores.EcoScoreScore,
                NovaGroup = product.Scores.NovaGroup,
                NutrientLevelsFat = product.Scores.NutrientLevelsFat,
                NutrientLevelsSaturatedFat = product.Scores.NutrientLevelsSaturatedFat,
                NutrientLevelsSugars = product.Scores.NutrientLevelsSugars,
                NutrientLevelsSalt = product.Scores.NutrientLevelsSalt
            } : null,
            Images = product.Images != null ? new OffImagesDto
            {
                ImageFrontUrl = product.Images.ImageFrontUrl,
                ImageNutritionUrl = product.Images.ImageNutritionUrl,
                ImageIngredientsUrl = product.Images.ImageIngredientsUrl,
                ImagePackagingUrl = product.Images.ImagePackagingUrl,
                ImagesCount = product.Images.ImagesCount
            } : null,
            Environment = product.Environment != null ? new OffEnvironmentDto
            {
                CarbonFootprint100g = product.Environment.CarbonFootprint100g,
                EcoScoreGrade = product.Scores?.EcoScoreGrade,
                AgribalyseCo2Total = product.Environment.AgribalyseCo2Total,
                PackagingRecycling = product.Environment.PackagingRecycling,
                Origins = product.Environment.Origins
            } : null
        };
    }
}

public record DatabaseStats
{
    public int LocalProductCount { get; init; }
    public int AvailableIsraeliProducts { get; init; }
    public double CoveragePercentage { get; init; }
}

public record OffProductDto
{
    public string Barcode { get; init; } = string.Empty;
    public string? ProductName { get; init; }
    public string? ProductNameHe { get; init; }
    public string? ProductNameEn { get; init; }
    public string? Brands { get; init; }
    public string? Quantity { get; init; }
    public string? Categories { get; init; }
    public decimal? Completeness { get; init; }
    public DateTime? LastSyncedAt { get; init; }

    public OffNutritionDto? Nutrition { get; init; }
    public OffIngredientsDto? Ingredients { get; init; }
    public OffScoresDto? Scores { get; init; }
    public OffImagesDto? Images { get; init; }
    public OffEnvironmentDto? Environment { get; init; }
}

public record OffNutritionDto
{
    public decimal? EnergyKcal100g { get; init; }
    public decimal? Fat100g { get; init; }
    public decimal? SaturatedFat100g { get; init; }
    public decimal? Carbohydrates100g { get; init; }
    public decimal? Sugars100g { get; init; }
    public decimal? Fiber100g { get; init; }
    public decimal? Proteins100g { get; init; }
    public decimal? Sodium100g { get; init; }
    public decimal? Salt100g { get; init; }
    public string? NutritionGradeFr { get; init; }
}

public record OffIngredientsDto
{
    public string? IngredientsText { get; init; }
    public int? IngredientsCount { get; init; }
    public List<string>? AllergensTags { get; init; }
    public List<string>? AdditivesTags { get; init; }
    public int? NovaGroup { get; init; }
    public bool? IsVegan { get; init; }
    public bool? IsVegetarian { get; init; }
    public bool? IsPalmOilFree { get; init; }
}

public record OffScoresDto
{
    public string? NutriScoreGrade { get; init; }
    public int? NutriScoreScore { get; init; }
    public string? EcoScoreGrade { get; init; }
    public int? EcoScoreScore { get; init; }
    public int? NovaGroup { get; init; }
    public string? NutrientLevelsFat { get; init; }
    public string? NutrientLevelsSaturatedFat { get; init; }
    public string? NutrientLevelsSugars { get; init; }
    public string? NutrientLevelsSalt { get; init; }
}

public record OffImagesDto
{
    public string? ImageFrontUrl { get; init; }
    public string? ImageNutritionUrl { get; init; }
    public string? ImageIngredientsUrl { get; init; }
    public string? ImagePackagingUrl { get; init; }
    public int? ImagesCount { get; init; }
}

public record OffEnvironmentDto
{
    public decimal? CarbonFootprint100g { get; init; }
    public string? EcoScoreGrade { get; init; }
    public decimal? AgribalyseCo2Total { get; init; }
    public string? PackagingRecycling { get; init; }
    public string? Origins { get; init; }
}

public record PullAllResult
{
    public int PulledCount { get; init; }
    public string Message { get; init; } = string.Empty;
}
