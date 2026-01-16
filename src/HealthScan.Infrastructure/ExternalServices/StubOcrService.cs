using HealthScan.Domain.Interfaces;
using HealthScan.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HealthScan.Infrastructure.ExternalServices;

public class StubOcrService : IOcrService
{
    private readonly ILogger<StubOcrService> _logger;

    public StubOcrService(ILogger<StubOcrService> logger)
    {
        _logger = logger;
    }

    public Task<OcrResult> ExtractTextAsync(byte[] imageData, string? languageHint = null, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("StubOcrService is being used. Implement a real OCR service (Azure, Google, Tesseract) for production.");

        return Task.FromResult(new OcrResult
        {
            Success = true,
            Confidence = 0.5,
            RawText = "[OCR Stub] Please implement a real OCR service. Image size: " + imageData.Length + " bytes"
        });
    }
}
