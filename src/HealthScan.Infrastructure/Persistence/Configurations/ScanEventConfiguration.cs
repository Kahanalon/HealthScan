using HealthScan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations;

public class ScanEventConfiguration : IEntityTypeConfiguration<ScanEvent>
{
    public void Configure(EntityTypeBuilder<ScanEvent> builder)
    {
        builder.ToTable("scan_events");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");

        builder.Property(s => s.Barcode).HasColumnName("barcode").HasMaxLength(20).IsRequired();
        builder.HasIndex(s => s.Barcode);

        builder.Property(s => s.DeviceId).HasColumnName("device_id").HasMaxLength(100);
        builder.Property(s => s.ScanType).HasColumnName("scan_type").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(s => s.Score).HasColumnName("score");
        builder.Property(s => s.Grade).HasColumnName("grade").HasConversion<string>().HasMaxLength(1);
        builder.Property(s => s.ScannedAt).HasColumnName("scanned_at").HasDefaultValueSql("NOW()");
        builder.HasIndex(s => s.ScannedAt);
    }
}
