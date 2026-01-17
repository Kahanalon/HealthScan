namespace HealthScan.Domain.Entities.OpenFoodFacts;

public class OffEnvironment
{
    public string Barcode { get; set; } = string.Empty;

    public decimal? CarbonFootprint100g { get; set; }
    public decimal? CarbonFootprintServing { get; set; }
    public string? CarbonFootprintUnit { get; set; }
    public string? CarbonFootprintSource { get; set; }

    public decimal? EnvironmentImpactLevel { get; set; }
    public List<string>? EnvironmentImpactLevelTags { get; set; }

    public string? PackagingRecycling { get; set; }
    public List<OffPackagingComponent>? PackagingComponents { get; set; }
    public string? PackagingMaterials { get; set; }
    public List<string>? PackagingMaterialsTags { get; set; }

    public string? RecyclingInstruction { get; set; }
    public string? RecyclingInstructionToDiscard { get; set; }
    public string? RecyclingInstructionToRecycle { get; set; }

    public string? Origins { get; set; }
    public List<string>? OriginsTags { get; set; }
    public string? ManufacturingPlaces { get; set; }
    public List<string>? ManufacturingPlacesTags { get; set; }

    public decimal? WaterFootprint100g { get; set; }
    public string? WaterFootprintUnit { get; set; }

    public int? AgribalyseFoodCode { get; set; }
    public string? AgribalyseFoodName { get; set; }
    public decimal? AgribalyseCo2Agriculture { get; set; }
    public decimal? AgribalyseCo2Consumption { get; set; }
    public decimal? AgribalyseCo2Distribution { get; set; }
    public decimal? AgribalyseCo2Packaging { get; set; }
    public decimal? AgribalyseCo2Processing { get; set; }
    public decimal? AgribalyseCo2Transportation { get; set; }
    public decimal? AgribalyseCo2Total { get; set; }
    public decimal? AgribalyseEfSingleScore { get; set; }

    public bool? IsForestFootprintFree { get; set; }
    public decimal? ForestFootprint { get; set; }

    public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;

    public OffProduct? Product { get; set; }
}

public class OffPackagingComponent
{
    public string? Shape { get; set; }
    public string? Material { get; set; }
    public string? Recycling { get; set; }
    public decimal? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public int? Quantity { get; set; }
}
