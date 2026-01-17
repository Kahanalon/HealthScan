using HealthScan.Domain.Entities.OpenFoodFacts;
using HealthScan.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HealthScan.Application.Services;

public interface IOffSyncService
{
    Task<OffProduct?> SyncProductAsync(string barcode, CancellationToken cancellationToken = default);
    Task<OffStats> GetStatsAsync(CancellationToken cancellationToken = default);
    Task<OffProduct?> GetProductAsync(string barcode, CancellationToken cancellationToken = default);
}

public class OffSyncService : IOffSyncService
{
    private readonly IOffDataSource _dataSource;
    private readonly IOffProductRepository _repository;
    private readonly ILogger<OffSyncService> _logger;

    public OffSyncService(
        IOffDataSource dataSource,
        IOffProductRepository repository,
        ILogger<OffSyncService> logger)
    {
        _dataSource = dataSource;
        _repository = repository;
        _logger = logger;
    }

    public async Task<OffProduct?> SyncProductAsync(string barcode, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Syncing OFF product: {Barcode}", barcode);

        var product = await _dataSource.GetFullProductByBarcodeAsync(barcode, cancellationToken);

        if (product == null)
        {
            _logger.LogWarning("Product not found in OFF: {Barcode}", barcode);
            return null;
        }

        await _repository.UpsertAsync(product, cancellationToken);

        _logger.LogInformation("OFF product synced successfully: {Barcode}", barcode);
        return product;
    }

    public async Task<OffStats> GetStatsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetStatsAsync(cancellationToken);
    }

    public async Task<OffProduct?> GetProductAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByBarcodeAsync(barcode, cancellationToken);
    }
}
