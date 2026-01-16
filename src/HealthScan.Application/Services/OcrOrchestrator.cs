using HealthScan.Application.DTOs;
using HealthScan.Domain.Interfaces;

namespace HealthScan.Application.Services;

public interface IOcrOrchestrator
{
    Task<NutritionOcrResponseDto> ProcessNutritionImageAsync(byte[] imageData, string? barcode, CancellationToken cancellationToken = default);
    Task<IngredientsOcrResponseDto> ProcessIngredientsImageAsync(byte[] imageData, string? barcode, CancellationToken cancellationToken = default);
}

public class OcrOrchestrator : IOcrOrchestrator
{
    private readonly IOcrService _ocrService;
    private readonly INutritionParser _nutritionParser;
    private readonly IIngredientAnalyzer _ingredientAnalyzer;

    public OcrOrchestrator(
        IOcrService ocrService,
        INutritionParser nutritionParser,
        IIngredientAnalyzer ingredientAnalyzer)
    {
        _ocrService = ocrService;
        _nutritionParser = nutritionParser;
        _ingredientAnalyzer = ingredientAnalyzer;
    }

    public async Task<NutritionOcrResponseDto> ProcessNutritionImageAsync(byte[] imageData, string? barcode, CancellationToken cancellationToken = default)
    {
        var ocrResult = await _ocrService.ExtractTextAsync(imageData, "he,en", cancellationToken);

        if (!ocrResult.Success)
        {
            return new NutritionOcrResponseDto
            {
                Success = false,
                ErrorMessage = ocrResult.ErrorMessage ?? "OCR processing failed"
            };
        }

        var nutritionData = _nutritionParser.ParseNutritionText(ocrResult.RawText);
        var confidence = _nutritionParser.GetConfidence();

        return new NutritionOcrResponseDto
        {
            Success = true,
            Confidence = confidence,
            ExtractedData = new NutritionPer100Dto
            {
                Energy = nutritionData.Energy,
                Fat = nutritionData.Fat,
                SaturatedFat = nutritionData.SaturatedFat,
                Carbohydrates = nutritionData.Carbohydrates,
                Sugars = nutritionData.Sugars,
                Fiber = nutritionData.Fiber,
                Protein = nutritionData.Protein,
                Sodium = nutritionData.Sodium
            },
            RequiresConfirmation = confidence < 0.8,
            RawText = ocrResult.RawText
        };
    }

    public async Task<IngredientsOcrResponseDto> ProcessIngredientsImageAsync(byte[] imageData, string? barcode, CancellationToken cancellationToken = default)
    {
        var ocrResult = await _ocrService.ExtractTextAsync(imageData, "he,en", cancellationToken);

        if (!ocrResult.Success)
        {
            return new IngredientsOcrResponseDto
            {
                Success = false,
                ErrorMessage = ocrResult.ErrorMessage ?? "OCR processing failed"
            };
        }

        var parsedIngredients = _ingredientAnalyzer.ParseIngredients(ocrResult.RawText);
        var flags = _ingredientAnalyzer.AnalyzeIngredients(ocrResult.RawText);

        return new IngredientsOcrResponseDto
        {
            Success = true,
            Confidence = ocrResult.Confidence,
            ExtractedText = ocrResult.RawText,
            ParsedIngredients = parsedIngredients,
            DetectedFlags = flags.Select(f => new FlagDto
            {
                Type = f.Type.ToString(),
                Description = f.Description
            }).ToList(),
            RequiresConfirmation = ocrResult.Confidence < 0.8
        };
    }
}
