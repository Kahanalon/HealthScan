namespace HealthScan.Application.DTOs;

public record NutritionDto
{
    public NutritionPer100Dto? Per100 { get; init; }
    public NutritionPer100Dto? PerServing { get; init; }
    public string? ServingSize { get; init; }
}

public record NutritionPer100Dto
{
    public decimal? Energy { get; init; }
    public decimal? Fat { get; init; }
    public decimal? SaturatedFat { get; init; }
    public decimal? Carbohydrates { get; init; }
    public decimal? Sugars { get; init; }
    public decimal? Fiber { get; init; }
    public decimal? Protein { get; init; }
    public decimal? Sodium { get; init; }
}
