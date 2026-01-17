using HealthScan.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HealthScan.Application.Services;

public interface IImportService
{
    Task<ImportResult> ImportIsraeliProductsAsync(int maxProducts = 1000, CancellationToken cancellationToken = default);
}

public record ImportResult
{
    public int TotalAvailable { get; init; }
    public int Imported { get; init; }
    public int Skipped { get; init; }
    public int Failed { get; init; }
    public int TotalInDatabase { get; init; }
    public TimeSpan Duration { get; init; }
}

public class ImportService : IImportService
{
    private readonly IProductDataSource _dataSource;
    private readonly IProductRepository _repository;
    private readonly ILogger<ImportService> _logger;
    private const int PageSize = 100;

    public ImportService(
        IProductDataSource dataSource,
        IProductRepository repository,
        ILogger<ImportService> logger)
    {
        _dataSource = dataSource;
        _repository = repository;
        _logger = logger;
    }

    public async Task<ImportResult> ImportIsraeliProductsAsync(int maxProducts = 1000, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        var totalAvailable = await _dataSource.GetIsraeliProductCountAsync(cancellationToken);

        _logger.LogInformation("Starting import of Israeli products. Total available: {Count}", totalAvailable);

        var imported = 0;
        var skipped = 0;
        var failed = 0;
        var page = 1;
        var productsToImport = Math.Min(maxProducts, totalAvailable);
        var totalPages = (int)Math.Ceiling((double)productsToImport / PageSize);

        while (imported + skipped < productsToImport && page <= totalPages)
        {
            try
            {
                var products = await _dataSource.FetchIsraeliProductsAsync(page, PageSize, cancellationToken);

                if (products.Count == 0)
                {
                    _logger.LogWarning("No products returned for page {Page}, stopping import", page);
                    break;
                }

                var insertedCount = await _repository.BulkInsertAsync(products, cancellationToken);
                var skippedCount = products.Count - insertedCount;

                imported += insertedCount;
                skipped += skippedCount;

                _logger.LogInformation(
                    "Page {Page}/{TotalPages}: Imported {Inserted}, Skipped {Skipped} (duplicates)",
                    page, totalPages, insertedCount, skippedCount);

                page++;

                await Task.Delay(100, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing page {Page}", page);
                failed += PageSize;
                page++;
            }
        }

        var totalInDb = await _repository.GetTotalCountAsync(cancellationToken);
        var duration = DateTime.UtcNow - startTime;

        _logger.LogInformation(
            "Import completed. Imported: {Imported}, Skipped: {Skipped}, Failed: {Failed}, Total in DB: {Total}, Duration: {Duration}",
            imported, skipped, failed, totalInDb, duration);

        return new ImportResult
        {
            TotalAvailable = totalAvailable,
            Imported = imported,
            Skipped = skipped,
            Failed = failed,
            TotalInDatabase = totalInDb,
            Duration = duration
        };
    }
}
