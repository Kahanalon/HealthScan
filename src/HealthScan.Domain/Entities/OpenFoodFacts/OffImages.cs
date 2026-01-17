namespace HealthScan.Domain.Entities.OpenFoodFacts;

public class OffImages
{
    public string Barcode { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }
    public string? ImageSmallUrl { get; set; }
    public string? ImageThumbUrl { get; set; }

    public string? ImageFrontUrl { get; set; }
    public string? ImageFrontSmallUrl { get; set; }
    public string? ImageFrontThumbUrl { get; set; }

    public string? ImageNutritionUrl { get; set; }
    public string? ImageNutritionSmallUrl { get; set; }
    public string? ImageNutritionThumbUrl { get; set; }

    public string? ImageIngredientsUrl { get; set; }
    public string? ImageIngredientsSmallUrl { get; set; }
    public string? ImageIngredientsThumbUrl { get; set; }

    public string? ImagePackagingUrl { get; set; }
    public string? ImagePackagingSmallUrl { get; set; }
    public string? ImagePackagingThumbUrl { get; set; }

    public List<OffImageMetadata>? SelectedImages { get; set; }
    public List<string>? ImagesKeys { get; set; }
    public int? ImagesCount { get; set; }

    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;

    public OffProduct? Product { get; set; }
}

public class OffImageMetadata
{
    public string? Type { get; set; }
    public string? Language { get; set; }
    public string? Display { get; set; }
    public string? Small { get; set; }
    public string? Thumb { get; set; }
}
