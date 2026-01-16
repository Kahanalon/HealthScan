namespace HealthScan.Domain.Entities;

public class ProductContribution
{
    public Guid Id { get; set; }
    public Guid? ProductId { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public string? FieldValue { get; set; }
    public string? ImageUrl { get; set; }
    public string? DeviceId { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Product? Product { get; set; }
}
