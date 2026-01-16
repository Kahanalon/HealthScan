using HealthScan.Domain.Entities;

namespace HealthScan.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Product>> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetSearchCountAsync(string query, CancellationToken cancellationToken = default);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string barcode, CancellationToken cancellationToken = default);
}
