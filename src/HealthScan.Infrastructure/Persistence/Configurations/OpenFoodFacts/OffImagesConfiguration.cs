using HealthScan.Domain.Entities.OpenFoodFacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations.OpenFoodFacts;

public class OffImagesConfiguration : IEntityTypeConfiguration<OffImages>
{
    public void Configure(EntityTypeBuilder<OffImages> builder)
    {
        builder.ToTable("off_images");

        builder.HasKey(i => i.Barcode);
        builder.Property(i => i.Barcode).HasColumnName("barcode").HasMaxLength(50).IsRequired();

        builder.Property(i => i.ImageUrl).HasColumnName("image_url").HasMaxLength(1000);
        builder.Property(i => i.ImageSmallUrl).HasColumnName("image_small_url").HasMaxLength(1000);
        builder.Property(i => i.ImageThumbUrl).HasColumnName("image_thumb_url").HasMaxLength(1000);

        builder.Property(i => i.ImageFrontUrl).HasColumnName("image_front_url").HasMaxLength(1000);
        builder.Property(i => i.ImageFrontSmallUrl).HasColumnName("image_front_small_url").HasMaxLength(1000);
        builder.Property(i => i.ImageFrontThumbUrl).HasColumnName("image_front_thumb_url").HasMaxLength(1000);

        builder.Property(i => i.ImageNutritionUrl).HasColumnName("image_nutrition_url").HasMaxLength(1000);
        builder.Property(i => i.ImageNutritionSmallUrl).HasColumnName("image_nutrition_small_url").HasMaxLength(1000);
        builder.Property(i => i.ImageNutritionThumbUrl).HasColumnName("image_nutrition_thumb_url").HasMaxLength(1000);

        builder.Property(i => i.ImageIngredientsUrl).HasColumnName("image_ingredients_url").HasMaxLength(1000);
        builder.Property(i => i.ImageIngredientsSmallUrl).HasColumnName("image_ingredients_small_url").HasMaxLength(1000);
        builder.Property(i => i.ImageIngredientsThumbUrl).HasColumnName("image_ingredients_thumb_url").HasMaxLength(1000);

        builder.Property(i => i.ImagePackagingUrl).HasColumnName("image_packaging_url").HasMaxLength(1000);
        builder.Property(i => i.ImagePackagingSmallUrl).HasColumnName("image_packaging_small_url").HasMaxLength(1000);
        builder.Property(i => i.ImagePackagingThumbUrl).HasColumnName("image_packaging_thumb_url").HasMaxLength(1000);

        builder.Property(i => i.SelectedImages).HasColumnName("selected_images").HasColumnType("jsonb");
        builder.Property(i => i.ImagesKeys).HasColumnName("images_keys").HasColumnType("jsonb");
        builder.Property(i => i.ImagesCount).HasColumnName("images_count");

        builder.Property(i => i.LastSyncedAt).HasColumnName("last_synced_at").HasDefaultValueSql("NOW()");

        builder.HasOne(i => i.Product)
            .WithOne(p => p.Images)
            .HasForeignKey<OffImages>(i => i.Barcode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
