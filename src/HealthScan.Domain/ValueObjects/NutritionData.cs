namespace HealthScan.Domain.ValueObjects;

public record NutritionData
{
    public decimal? Energy { get; init; }
    public decimal? Fat { get; init; }
    public decimal? SaturatedFat { get; init; }
    public decimal? Carbohydrates { get; init; }
    public decimal? Sugars { get; init; }
    public decimal? Fiber { get; init; }
    public decimal? Protein { get; init; }
    public decimal? Sodium { get; init; }

    public bool HasRequiredFieldsForScoring()
    {
        return Sugars.HasValue && Sodium.HasValue && SaturatedFat.HasValue;
    }

    public List<string> GetMissingFields()
    {
        var missing = new List<string>();
        if (!Sugars.HasValue) missing.Add("sugars_100g");
        if (!Sodium.HasValue) missing.Add("sodium_100g");
        if (!SaturatedFat.HasValue) missing.Add("saturated_fat_100g");
        return missing;
    }
}
