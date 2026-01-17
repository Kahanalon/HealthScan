namespace HealthScan.Domain.Entities.OpenFoodFacts;

public class OffNutrition
{
    public string Barcode { get; set; } = string.Empty;

    public decimal? EnergyKcal100g { get; set; }
    public decimal? EnergyKj100g { get; set; }
    public decimal? EnergyKcalServing { get; set; }
    public decimal? EnergyKjServing { get; set; }

    public decimal? Fat100g { get; set; }
    public decimal? FatServing { get; set; }
    public decimal? SaturatedFat100g { get; set; }
    public decimal? SaturatedFatServing { get; set; }
    public decimal? MonounsaturatedFat100g { get; set; }
    public decimal? PolyunsaturatedFat100g { get; set; }
    public decimal? TransFat100g { get; set; }
    public decimal? Cholesterol100g { get; set; }
    public decimal? Omega3Fat100g { get; set; }
    public decimal? Omega6Fat100g { get; set; }

    public decimal? Carbohydrates100g { get; set; }
    public decimal? CarbohydratesServing { get; set; }
    public decimal? Sugars100g { get; set; }
    public decimal? SugarsServing { get; set; }
    public decimal? Starch100g { get; set; }
    public decimal? Polyols100g { get; set; }

    public decimal? Fiber100g { get; set; }
    public decimal? FiberServing { get; set; }

    public decimal? Proteins100g { get; set; }
    public decimal? ProteinsServing { get; set; }

    public decimal? Salt100g { get; set; }
    public decimal? SaltServing { get; set; }
    public decimal? Sodium100g { get; set; }
    public decimal? SodiumServing { get; set; }

    public decimal? VitaminA100g { get; set; }
    public decimal? VitaminB1100g { get; set; }
    public decimal? VitaminB2100g { get; set; }
    public decimal? VitaminB6100g { get; set; }
    public decimal? VitaminB9100g { get; set; }
    public decimal? VitaminB12100g { get; set; }
    public decimal? VitaminC100g { get; set; }
    public decimal? VitaminD100g { get; set; }
    public decimal? VitaminE100g { get; set; }
    public decimal? VitaminK100g { get; set; }
    public decimal? VitaminPp100g { get; set; }

    public decimal? Calcium100g { get; set; }
    public decimal? Iron100g { get; set; }
    public decimal? Magnesium100g { get; set; }
    public decimal? Zinc100g { get; set; }
    public decimal? Phosphorus100g { get; set; }
    public decimal? Potassium100g { get; set; }
    public decimal? Iodine100g { get; set; }
    public decimal? Selenium100g { get; set; }
    public decimal? Copper100g { get; set; }
    public decimal? Manganese100g { get; set; }
    public decimal? Fluoride100g { get; set; }

    public decimal? Caffeine100g { get; set; }
    public decimal? Taurine100g { get; set; }
    public decimal? Alcohol100g { get; set; }

    public string? NutritionDataPer { get; set; }
    public string? NutritionGradeFr { get; set; }

    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;

    public OffProduct? Product { get; set; }
}
