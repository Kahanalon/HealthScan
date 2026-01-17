using HealthScan.Domain.Entities.OpenFoodFacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations.OpenFoodFacts;

public class OffProductConfiguration : IEntityTypeConfiguration<OffProduct>
{
    public void Configure(EntityTypeBuilder<OffProduct> builder)
    {
        builder.ToTable("off_products");

        builder.HasKey(p => p.Barcode);
        builder.Property(p => p.Barcode).HasColumnName("barcode").HasMaxLength(50).IsRequired();

        builder.Property(p => p.ProductName).HasColumnName("product_name").HasMaxLength(1000);
        builder.Property(p => p.ProductNameHe).HasColumnName("product_name_he").HasMaxLength(1000);
        builder.Property(p => p.ProductNameEn).HasColumnName("product_name_en").HasMaxLength(1000);
        builder.Property(p => p.GenericName).HasColumnName("generic_name").HasMaxLength(1000);
        builder.Property(p => p.GenericNameHe).HasColumnName("generic_name_he").HasMaxLength(1000);
        builder.Property(p => p.GenericNameEn).HasColumnName("generic_name_en").HasMaxLength(1000);

        builder.Property(p => p.Brands).HasColumnName("brands").HasMaxLength(500);
        builder.Property(p => p.BrandsTags).HasColumnName("brands_tags").HasMaxLength(500);
        builder.Property(p => p.Quantity).HasColumnName("quantity").HasMaxLength(200);
        builder.Property(p => p.ServingSize).HasColumnName("serving_size").HasMaxLength(200);
        builder.Property(p => p.ServingQuantity).HasColumnName("serving_quantity").HasPrecision(10, 4);

        builder.Property(p => p.Categories).HasColumnName("categories");
        builder.Property(p => p.CategoriesTags).HasColumnName("categories_tags").HasColumnType("jsonb");
        builder.Property(p => p.CategoriesHierarchy).HasColumnName("categories_hierarchy");

        builder.Property(p => p.Labels).HasColumnName("labels").HasMaxLength(1000);
        builder.Property(p => p.LabelsTags).HasColumnName("labels_tags").HasColumnType("jsonb");

        builder.Property(p => p.Stores).HasColumnName("stores").HasMaxLength(500);
        builder.Property(p => p.Countries).HasColumnName("countries").HasMaxLength(500);
        builder.Property(p => p.CountriesTags).HasColumnName("countries_tags").HasColumnType("jsonb");

        builder.Property(p => p.ManufacturingPlaces).HasColumnName("manufacturing_places").HasMaxLength(500);
        builder.Property(p => p.Origins).HasColumnName("origins").HasMaxLength(500);
        builder.Property(p => p.Packaging).HasColumnName("packaging");
        builder.Property(p => p.PackagingTags).HasColumnName("packaging_tags").HasColumnType("jsonb");

        builder.Property(p => p.Completeness).HasColumnName("completeness").HasPrecision(5, 4);
        builder.Property(p => p.LastModifiedT).HasColumnName("last_modified_t");
        builder.Property(p => p.CreatedT).HasColumnName("created_t");

        builder.Property(p => p.Creator).HasColumnName("creator").HasMaxLength(200);
        builder.Property(p => p.Editor).HasColumnName("editor").HasMaxLength(200);
        builder.Property(p => p.EditorsCount).HasColumnName("editors_count");

        builder.Property(p => p.States).HasColumnName("states");
        builder.Property(p => p.StatesTags).HasColumnName("states_tags").HasColumnType("jsonb");

        builder.Property(p => p.ImportedAt).HasColumnName("imported_at").HasDefaultValueSql("NOW()");
        builder.Property(p => p.LastSyncedAt).HasColumnName("last_synced_at").HasDefaultValueSql("NOW()");

        builder.HasIndex(p => p.Brands);
        builder.HasIndex(p => p.Completeness);
        builder.HasIndex(p => p.LastSyncedAt);
    }
}
