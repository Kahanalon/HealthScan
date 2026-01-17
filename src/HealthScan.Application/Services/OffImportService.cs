using HealthScan.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HealthScan.Application.Services;

public interface IOffImportService
{
    Task<OffImportResult> ImportIsraeliProductsAsync(int maxProducts = 10000, CancellationToken cancellationToken = default);
}

public record OffImportResult
{
    public int TotalAvailable { get; init; }
    public int Imported { get; init; }
    public int Updated { get; init; }
    public int Failed { get; init; }
    public int TotalInDatabase { get; init; }
    public TimeSpan Duration { get; init; }
}

public class OffImportService : IOffImportService
{
    private readonly IOffDataSource _dataSource;
    private readonly IOffProductRepository _repository;
    private readonly ILogger<OffImportService> _logger;
    private const int PageSize = 100;

    public OffImportService(
        IOffDataSource dataSource,
        IOffProductRepository repository,
        ILogger<OffImportService> logger)
    {
        _dataSource = dataSource;
        _repository = repository;
        _logger = logger;
    }

    public async Task<OffImportResult> ImportIsraeliProductsAsync(int maxProducts = 10000, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        var totalAvailable = await _dataSource.GetIsraeliProductCountAsync(cancellationToken);

        _logger.LogInformation("Starting OFF import of Israeli products. Total available: {Count}", totalAvailable);

        var imported = 0;
        var updated = 0;
        var failed = 0;
        var page = 1;
        var productsToImport = Math.Min(maxProducts, totalAvailable);
        var totalPages = (int)Math.Ceiling((double)productsToImport / PageSize);

        while ((imported + updated) < productsToImport && page <= totalPages)
        {
            try
            {
                var products = await _dataSource.FetchFullIsraeliProductsAsync(page, PageSize, cancellationToken);

                if (products.Count == 0)
                {
                    _logger.LogWarning("No products returned for page {Page}, stopping import", page);
                    break;
                }

                var existingCount = await _repository.GetTotalCountAsync(cancellationToken);
                var upsertedCount = await _repository.BulkUpsertAsync(products, cancellationToken);
                var newCount = await _repository.GetTotalCountAsync(cancellationToken);

                var newlyImported = newCount - existingCount;
                var updatedCount = upsertedCount - newlyImported;

                imported += newlyImported;
                updated += Math.Max(0, updatedCount);

                _logger.LogInformation(
                    "Page {Page}/{TotalPages}: Imported {Imported}, Updated {Updated}",
                    page, totalPages, newlyImported, updatedCount);

                page++;

                await Task.Delay(200, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing OFF page {Page}", page);
                failed += PageSize;
                page++;
            }
        }

        var totalInDb = await _repository.GetTotalCountAsync(cancellationToken);
        var duration = DateTime.UtcNow - startTime;

        _logger.LogInformation(
            "OFF import completed. Imported: {Imported}, Updated: {Updated}, Failed: {Failed}, Total in DB: {Total}, Duration: {Duration}",
            imported, updated, failed, totalInDb, duration);

        return new OffImportResult
        {
            TotalAvailable = totalAvailable,
            Imported = imported,
            Updated = updated,
            Failed = failed,
            TotalInDatabase = totalInDb,
            Duration = duration
        };
    }
}
