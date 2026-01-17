using HealthScan.Domain.Entities;

namespace HealthScan.Domain.Interfaces;

public interface IProductDataSource
{
    string SourceName { get; }
    Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<List<Product>> SearchAsync(string query, int limit = 10, CancellationToken cancellationToken = default);
    Task<List<Product>> FetchIsraeliProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetIsraeliProductCountAsync(CancellationToken cancellationToken = default);
}
