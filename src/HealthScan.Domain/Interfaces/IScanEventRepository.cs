using HealthScan.Domain.Entities;

namespace HealthScan.Domain.Interfaces;

public interface IScanEventRepository
{
    Task<ScanEvent> AddAsync(ScanEvent scanEvent, CancellationToken cancellationToken = default);
    Task<List<ScanEvent>> GetByDeviceIdAsync(string deviceId, int limit = 100, CancellationToken cancellationToken = default);
    Task<int> GetScanCountAsync(string barcode, CancellationToken cancellationToken = default);
}
