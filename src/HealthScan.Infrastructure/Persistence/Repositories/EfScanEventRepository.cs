using HealthScan.Domain.Entities;
using HealthScan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthScan.Infrastructure.Persistence.Repositories;

public class EfScanEventRepository : IScanEventRepository
{
    private readonly AppDbContext _context;

    public EfScanEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ScanEvent> AddAsync(ScanEvent scanEvent, CancellationToken cancellationToken = default)
    {
        scanEvent.Id = Guid.NewGuid();
        scanEvent.ScannedAt = DateTime.UtcNow;

        _context.ScanEvents.Add(scanEvent);
        await _context.SaveChangesAsync(cancellationToken);

        return scanEvent;
    }

    public async Task<List<ScanEvent>> GetByDeviceIdAsync(string deviceId, int limit = 100, CancellationToken cancellationToken = default)
    {
        return await _context.ScanEvents
            .Where(s => s.DeviceId == deviceId)
            .OrderByDescending(s => s.ScannedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetScanCountAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return await _context.ScanEvents
            .CountAsync(s => s.Barcode == barcode, cancellationToken);
    }
}
