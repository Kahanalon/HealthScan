using HealthScan.Domain.ValueObjects;

namespace HealthScan.Domain.Interfaces;

public interface IOcrService
{
    Task<OcrResult> ExtractTextAsync(byte[] imageData, string? languageHint = null, CancellationToken cancellationToken = default);
}
