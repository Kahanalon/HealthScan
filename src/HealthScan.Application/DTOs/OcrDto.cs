namespace HealthScan.Application.DTOs;

public record OcrRequestDto
{
    public string ImageBase64 { get; init; } = string.Empty;
    public string? Barcode { get; init; }
}

public record NutritionOcrResponseDto
{
    public bool Success { get; init; }
    public double Confidence { get; init; }
    public NutritionPer100Dto? ExtractedData { get; init; }
    public bool RequiresConfirmation { get; init; }
    public string? RawText { get; init; }
    public string? ErrorMessage { get; init; }
}

public record IngredientsOcrResponseDto
{
    public bool Success { get; init; }
    public double Confidence { get; init; }
    public string ExtractedText { get; init; } = string.Empty;
    public List<string> ParsedIngredients { get; init; } = new();
    public List<FlagDto> DetectedFlags { get; init; } = new();
    public bool RequiresConfirmation { get; init; }
    public string? ErrorMessage { get; init; }
}
