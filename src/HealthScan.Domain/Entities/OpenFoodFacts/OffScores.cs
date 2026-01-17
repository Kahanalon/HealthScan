namespace HealthScan.Domain.Entities.OpenFoodFacts;

public class OffScores
{
    public string Barcode { get; set; } = string.Empty;

    public string? NutriScoreGrade { get; set; }
    public int? NutriScoreScore { get; set; }
    public string? NutriScoreVersion { get; set; }

    public string? NutriscoreGrade2021 { get; set; }
    public int? NutriscoreScore2021 { get; set; }
    public int? NutriscoreNegativePoints2021 { get; set; }
    public int? NutriscorePositivePoints2021 { get; set; }

    public string? NutriscoreGrade2023 { get; set; }
    public int? NutriscoreScore2023 { get; set; }
    public int? NutriscoreNegativePoints2023 { get; set; }
    public int? NutriscorePositivePoints2023 { get; set; }

    public string? EcoScoreGrade { get; set; }
    public int? EcoScoreScore { get; set; }
    public string? EcoScoreVersion { get; set; }

    public int? EcoScoreAdjustments { get; set; }
    public int? EcoScorePackaging { get; set; }
    public int? EcoScoreProduction { get; set; }
    public int? EcoScoreOrigins { get; set; }
    public int? EcoScoreThreatenedSpecies { get; set; }

    public int? NovaGroup { get; set; }

    public decimal? NutrientLevelsEnergy { get; set; }
    public string? NutrientLevelsFat { get; set; }
    public string? NutrientLevelsSaturatedFat { get; set; }
    public string? NutrientLevelsSugars { get; set; }
    public string? NutrientLevelsSalt { get; set; }

    public int? DataQualityErrorsCount { get; set; }
    public List<string>? DataQualityErrorsTags { get; set; }
    public int? DataQualityWarningsCount { get; set; }
    public List<string>? DataQualityWarningsTags { get; set; }
    public int? DataQualityInfoCount { get; set; }
    public List<string>? DataQualityInfoTags { get; set; }

    public decimal? UnknownNutrientsCount { get; set; }
    public decimal? KnownNutrientsCount { get; set; }

    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;

    public OffProduct? Product { get; set; }
}
