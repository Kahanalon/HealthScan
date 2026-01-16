using HealthScan.Domain.Enums;

namespace HealthScan.Domain.Entities;

public class ScanEvent
{
    public Guid Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string? DeviceId { get; set; }
    public ScanType ScanType { get; set; }
    public int? Score { get; set; }
    public Grade? Grade { get; set; }
    public DateTime ScannedAt { get; set; } = DateTime.UtcNow;
}
