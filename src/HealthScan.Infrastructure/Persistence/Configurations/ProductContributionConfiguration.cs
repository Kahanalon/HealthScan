using HealthScan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations;

public class ProductContributionConfiguration : IEntityTypeConfiguration<ProductContribution>
{
    public void Configure(EntityTypeBuilder<ProductContribution> builder)
    {
        builder.ToTable("product_contributions");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.ProductId).HasColumnName("product_id");
        builder.Property(c => c.Barcode).HasColumnName("barcode").HasMaxLength(20).IsRequired();
        builder.HasIndex(c => c.Barcode);

        builder.Property(c => c.FieldName).HasColumnName("field_name").HasMaxLength(100).IsRequired();
        builder.Property(c => c.FieldValue).HasColumnName("field_value");
        builder.Property(c => c.ImageUrl).HasColumnName("image_url").HasMaxLength(1000);
        builder.Property(c => c.DeviceId).HasColumnName("device_id").HasMaxLength(100);
        builder.Property(c => c.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("pending");
        builder.HasIndex(c => c.Status);

        builder.Property(c => c.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

        builder.HasOne(c => c.Product)
            .WithMany()
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
