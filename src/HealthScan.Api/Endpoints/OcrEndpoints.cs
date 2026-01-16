using HealthScan.Application.DTOs;
using HealthScan.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthScan.Api.Endpoints;

public static class OcrEndpoints
{
    public static void MapOcrEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/ocr")
            .WithTags("OCR")
            .WithOpenApi();

        group.MapPost("/nutrition", ProcessNutrition)
            .WithName("ProcessNutritionImage")
            .WithSummary("Process nutrition label image with OCR")
            .Produces<NutritionOcrResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/ingredients", ProcessIngredients)
            .WithName("ProcessIngredientsImage")
            .WithSummary("Process ingredients label image with OCR")
            .Produces<IngredientsOcrResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> ProcessNutrition(
        [FromBody] OcrRequestDto request,
        [FromServices] IOcrOrchestrator ocrOrchestrator,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ImageBase64))
        {
            return Results.BadRequest(new { error = "Image data is required" });
        }

        byte[] imageData;
        try
        {
            var base64Data = request.ImageBase64;
            if (base64Data.Contains(","))
            {
                base64Data = base64Data.Split(',')[1];
            }
            imageData = Convert.FromBase64String(base64Data);
        }
        catch (FormatException)
        {
            return Results.BadRequest(new { error = "Invalid base64 image data" });
        }

        var result = await ocrOrchestrator.ProcessNutritionImageAsync(imageData, request.Barcode, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> ProcessIngredients(
        [FromBody] OcrRequestDto request,
        [FromServices] IOcrOrchestrator ocrOrchestrator,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ImageBase64))
        {
            return Results.BadRequest(new { error = "Image data is required" });
        }

        byte[] imageData;
        try
        {
            var base64Data = request.ImageBase64;
            if (base64Data.Contains(","))
            {
                base64Data = base64Data.Split(',')[1];
            }
            imageData = Convert.FromBase64String(base64Data);
        }
        catch (FormatException)
        {
            return Results.BadRequest(new { error = "Invalid base64 image data" });
        }

        var result = await ocrOrchestrator.ProcessIngredientsImageAsync(imageData, request.Barcode, cancellationToken);
        return Results.Ok(result);
    }
}
