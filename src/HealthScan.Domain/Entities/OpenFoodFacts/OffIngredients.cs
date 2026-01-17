namespace HealthScan.Domain.Entities.OpenFoodFacts;

public class OffIngredients
{
    public string Barcode { get; set; } = string.Empty;

    public string? IngredientsText { get; set; }
    public string? IngredientsTextHe { get; set; }
    public string? IngredientsTextEn { get; set; }

    public List<OffIngredientItem>? IngredientsParsed { get; set; }
    public int? IngredientsCount { get; set; }
    public decimal? IngredientsPercentAnalysis { get; set; }

    public List<string>? Allergens { get; set; }
    public List<string>? AllergensTags { get; set; }
    public string? AllergensHierarchy { get; set; }

    public List<string>? Traces { get; set; }
    public List<string>? TracesTags { get; set; }

    public List<string>? Additives { get; set; }
    public List<string>? AdditivesTags { get; set; }
    public int? AdditivesCount { get; set; }

    public List<string>? AminoAcidsTags { get; set; }
    public List<string>? MineralsTags { get; set; }
    public List<string>? VitaminsTags { get; set; }
    public List<string>? NucleotidesTags { get; set; }
    public List<string>? OtherNutritionalSubstancesTags { get; set; }

    public int? NovaGroup { get; set; }
    public string? NovaGroupsMarkers { get; set; }
    public List<string>? NovaGroupsTags { get; set; }

    public bool? IsPalmOilFree { get; set; }
    public bool? IsVegan { get; set; }
    public bool? IsVegetarian { get; set; }
    public string? VeganAnalysis { get; set; }
    public string? VegetarianAnalysis { get; set; }

    public string? IngredientsAnalysis { get; set; }
    public List<string>? IngredientsAnalysisTags { get; set; }

    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;

    public OffProduct? Product { get; set; }
}

public class OffIngredientItem
{
    public string? Id { get; set; }
    public string? Text { get; set; }
    public decimal? Percent { get; set; }
    public decimal? PercentMin { get; set; }
    public decimal? PercentMax { get; set; }
    public decimal? PercentEstimate { get; set; }
    public string? Vegan { get; set; }
    public string? Vegetarian { get; set; }
    public bool? FromPalmOil { get; set; }
    public List<OffIngredientItem>? Ingredients { get; set; }
}
