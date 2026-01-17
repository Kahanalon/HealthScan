namespace HealthScan.Domain.Entities.OpenFoodFacts;

public class OffProduct
{
    public string Barcode { get; set; } = string.Empty;

    public string? ProductName { get; set; }
    public string? ProductNameHe { get; set; }
    public string? ProductNameEn { get; set; }
    public string? GenericName { get; set; }
    public string? GenericNameHe { get; set; }
    public string? GenericNameEn { get; set; }

    public string? Brands { get; set; }
    public string? BrandsTags { get; set; }
    public string? Quantity { get; set; }
    public string? ServingSize { get; set; }
    public decimal? ServingQuantity { get; set; }

    public string? Categories { get; set; }
    public List<string>? CategoriesTags { get; set; }
    public string? CategoriesHierarchy { get; set; }

    public string? Labels { get; set; }
    public List<string>? LabelsTags { get; set; }

    public string? Stores { get; set; }
    public string? Countries { get; set; }
    public List<string>? CountriesTags { get; set; }

    public string? ManufacturingPlaces { get; set; }
    public string? Origins { get; set; }
    public string? Packaging { get; set; }
    public List<string>? PackagingTags { get; set; }

    public decimal? Completeness { get; set; }
    public DateTime? LastModifiedT { get; set; }
    public DateTime? CreatedT { get; set; }

    public string? Creator { get; set; }
    public string? Editor { get; set; }
    public int? EditorsCount { get; set; }

    public string? States { get; set; }
    public List<string>? StatesTags { get; set; }

    public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;

    public OffNutrition? Nutrition { get; set; }
    public OffIngredients? Ingredients { get; set; }
    public OffScores? Scores { get; set; }
    public OffImages? Images { get; set; }
    public OffEnvironment? Environment { get; set; }
}
