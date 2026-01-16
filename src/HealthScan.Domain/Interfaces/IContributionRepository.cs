using HealthScan.Domain.Entities;

namespace HealthScan.Domain.Interfaces;

public interface IContributionRepository
{
    Task<ProductContribution> AddAsync(ProductContribution contribution, CancellationToken cancellationToken = default);
    Task<List<ProductContribution>> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<List<ProductContribution>> GetPendingAsync(int limit = 100, CancellationToken cancellationToken = default);
    Task<ProductContribution> UpdateStatusAsync(Guid id, string status, CancellationToken cancellationToken = default);
}
