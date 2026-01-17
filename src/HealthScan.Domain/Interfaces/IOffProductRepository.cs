using HealthScan.Domain.Entities.OpenFoodFacts;

namespace HealthScan.Domain.Interfaces;

public interface IOffProductRepository
{
    Task<OffProduct?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<List<OffProduct>> GetAllAsync(int skip = 0, int take = 100, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<int> BulkUpsertAsync(List<OffProduct> products, CancellationToken cancellationToken = default);
    Task<bool> UpsertAsync(OffProduct product, CancellationToken cancellationToken = default);
    Task<OffStats> GetStatsAsync(CancellationToken cancellationToken = default);
}

public record OffStats
{
    public int TotalProducts { get; init; }
    public int WithNutriScore { get; init; }
    public int WithEcoScore { get; init; }
    public int WithNovaGroup { get; init; }
    public int WithIngredients { get; init; }
    public int WithAllergens { get; init; }
    public int WithImages { get; init; }
    public DateTime? LastSyncedAt { get; init; }
    public Dictionary<string, int> NutriScoreDistribution { get; init; } = new();
    public Dictionary<int, int> NovaGroupDistribution { get; init; } = new();
}
