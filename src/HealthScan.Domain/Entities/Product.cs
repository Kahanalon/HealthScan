using HealthScan.Domain.Enums;
using HealthScan.Domain.ValueObjects;

namespace HealthScan.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string? NameHe { get; set; }
    public string? NameEn { get; set; }
    public string? Brand { get; set; }
    public string? PackageSize { get; set; }
    public string? Category { get; set; }

    public decimal? Energy100g { get; set; }
    public decimal? Fat100g { get; set; }
    public decimal? SaturatedFat100g { get; set; }
    public decimal? Carbohydrates100g { get; set; }
    public decimal? Sugars100g { get; set; }
    public decimal? Fiber100g { get; set; }
    public decimal? Protein100g { get; set; }
    public decimal? Sodium100g { get; set; }

    public string? ServingSize { get; set; }
    public decimal? EnergyServing { get; set; }
    public decimal? FatServing { get; set; }
    public decimal? SaturatedFatServing { get; set; }
    public decimal? CarbohydratesServing { get; set; }
    public decimal? SugarsServing { get; set; }
    public decimal? FiberServing { get; set; }
    public decimal? ProteinServing { get; set; }
    public decimal? SodiumServing { get; set; }

    public string? IngredientsTextHe { get; set; }
    public string? IngredientsTextEn { get; set; }
    public List<string>? IngredientsParsed { get; set; }
    public List<string>? Allergens { get; set; }

    public string? ImageFrontUrl { get; set; }
    public string? ImageNutritionUrl { get; set; }
    public string? ImageIngredientsUrl { get; set; }

    public string Source { get; set; } = "user";
    public ProductStatus Status { get; set; } = ProductStatus.Pending;
    public bool NutritionComplete { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? OffBarcode { get; set; }
    public DateTime? OffSyncedAt { get; set; }

    public string DisplayName => NameHe ?? NameEn ?? "Unknown Product";

    public NutritionData GetNutritionPer100()
    {
        return new NutritionData
        {
            Energy = Energy100g,
            Fat = Fat100g,
            SaturatedFat = SaturatedFat100g,
            Carbohydrates = Carbohydrates100g,
            Sugars = Sugars100g,
            Fiber = Fiber100g,
            Protein = Protein100g,
            Sodium = Sodium100g
        };
    }

    public void UpdateNutrition(NutritionData data)
    {
        Energy100g = data.Energy;
        Fat100g = data.Fat;
        SaturatedFat100g = data.SaturatedFat;
        Carbohydrates100g = data.Carbohydrates;
        Sugars100g = data.Sugars;
        Fiber100g = data.Fiber;
        Protein100g = data.Protein;
        Sodium100g = data.Sodium;
        NutritionComplete = data.HasRequiredFieldsForScoring();
        LastUpdated = DateTime.UtcNow;
    }
}
