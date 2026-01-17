using HealthScan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.Barcode).HasColumnName("barcode").HasMaxLength(50).IsRequired();
        builder.HasIndex(p => p.Barcode).IsUnique();

        builder.Property(p => p.NameHe).HasColumnName("name_he").HasMaxLength(500);
        builder.Property(p => p.NameEn).HasColumnName("name_en").HasMaxLength(500);
        builder.Property(p => p.Brand).HasColumnName("brand").HasMaxLength(500);
        builder.Property(p => p.PackageSize).HasColumnName("package_size").HasMaxLength(300);
        builder.Property(p => p.Category).HasColumnName("category").HasMaxLength(500);

        builder.Property(p => p.Energy100g).HasColumnName("energy_100g").HasPrecision(10, 2);
        builder.Property(p => p.Fat100g).HasColumnName("fat_100g").HasPrecision(10, 2);
        builder.Property(p => p.SaturatedFat100g).HasColumnName("saturated_fat_100g").HasPrecision(10, 2);
        builder.Property(p => p.Carbohydrates100g).HasColumnName("carbohydrates_100g").HasPrecision(10, 2);
        builder.Property(p => p.Sugars100g).HasColumnName("sugars_100g").HasPrecision(10, 2);
        builder.Property(p => p.Fiber100g).HasColumnName("fiber_100g").HasPrecision(10, 2);
        builder.Property(p => p.Protein100g).HasColumnName("protein_100g").HasPrecision(10, 2);
        builder.Property(p => p.Sodium100g).HasColumnName("sodium_100g").HasPrecision(10, 2);

        builder.Property(p => p.ServingSize).HasColumnName("serving_size").HasMaxLength(100);
        builder.Property(p => p.EnergyServing).HasColumnName("energy_serving").HasPrecision(10, 2);
        builder.Property(p => p.FatServing).HasColumnName("fat_serving").HasPrecision(10, 2);
        builder.Property(p => p.SaturatedFatServing).HasColumnName("saturated_fat_serving").HasPrecision(10, 2);
        builder.Property(p => p.CarbohydratesServing).HasColumnName("carbohydrates_serving").HasPrecision(10, 2);
        builder.Property(p => p.SugarsServing).HasColumnName("sugars_serving").HasPrecision(10, 2);
        builder.Property(p => p.FiberServing).HasColumnName("fiber_serving").HasPrecision(10, 2);
        builder.Property(p => p.ProteinServing).HasColumnName("protein_serving").HasPrecision(10, 2);
        builder.Property(p => p.SodiumServing).HasColumnName("sodium_serving").HasPrecision(10, 2);

        builder.Property(p => p.IngredientsTextHe).HasColumnName("ingredients_text_he");
        builder.Property(p => p.IngredientsTextEn).HasColumnName("ingredients_text_en");
        builder.Property(p => p.IngredientsParsed).HasColumnName("ingredients_parsed").HasColumnType("jsonb");
        builder.Property(p => p.Allergens).HasColumnName("allergens").HasColumnType("jsonb");

        builder.Property(p => p.ImageFrontUrl).HasColumnName("image_front_url").HasMaxLength(1000);
        builder.Property(p => p.ImageNutritionUrl).HasColumnName("image_nutrition_url").HasMaxLength(1000);
        builder.Property(p => p.ImageIngredientsUrl).HasColumnName("image_ingredients_url").HasMaxLength(1000);

        builder.Property(p => p.Source).HasColumnName("source").HasMaxLength(50).HasDefaultValue("user");
        builder.Property(p => p.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(p => p.Status);

        builder.Property(p => p.NutritionComplete).HasColumnName("nutrition_complete").HasDefaultValue(false);
        builder.Property(p => p.LastUpdated).HasColumnName("last_updated").HasDefaultValueSql("NOW()");
        builder.Property(p => p.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");

        builder.Property(p => p.OffBarcode).HasColumnName("off_barcode").HasMaxLength(50);
        builder.Property(p => p.OffSyncedAt).HasColumnName("off_synced_at");
        builder.HasIndex(p => p.OffBarcode);
    }
}
