namespace HealthScan.Domain.ValueObjects;

public record OcrResult
{
    public bool Success { get; init; }
    public double Confidence { get; init; }
    public string RawText { get; init; } = string.Empty;
    public string? ErrorMessage { get; init; }
}

public record NutritionOcrResult : OcrResult
{
    public NutritionData? ExtractedData { get; init; }
    public bool RequiresConfirmation { get; init; }
}

public record IngredientsOcrResult : OcrResult
{
    public string ExtractedText { get; init; } = string.Empty;
    public List<string> ParsedIngredients { get; init; } = new();
    public List<ScoreFlag> DetectedFlags { get; init; } = new();
    public bool RequiresConfirmation { get; init; }
}
