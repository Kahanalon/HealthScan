using HealthScan.Domain.Entities.OpenFoodFacts;

namespace HealthScan.Domain.Interfaces;

public interface IOffDataSource
{
    Task<OffProduct?> GetFullProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<List<OffProduct>> FetchFullIsraeliProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetIsraeliProductCountAsync(CancellationToken cancellationToken = default);
}
