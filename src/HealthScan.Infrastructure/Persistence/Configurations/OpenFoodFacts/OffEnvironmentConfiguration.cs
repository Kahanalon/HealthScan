using HealthScan.Domain.Entities.OpenFoodFacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations.OpenFoodFacts;

public class OffEnvironmentConfiguration : IEntityTypeConfiguration<OffEnvironment>
{
    public void Configure(EntityTypeBuilder<OffEnvironment> builder)
    {
        builder.ToTable("off_environment");

        builder.HasKey(e => e.Barcode);
        builder.Property(e => e.Barcode).HasColumnName("barcode").HasMaxLength(50).IsRequired();

        builder.Property(e => e.CarbonFootprint100g).HasColumnName("carbon_footprint_100g").HasPrecision(12, 4);
        builder.Property(e => e.CarbonFootprintServing).HasColumnName("carbon_footprint_serving").HasPrecision(12, 4);
        builder.Property(e => e.CarbonFootprintUnit).HasColumnName("carbon_footprint_unit").HasMaxLength(50);
        builder.Property(e => e.CarbonFootprintSource).HasColumnName("carbon_footprint_source").HasMaxLength(200);

        builder.Property(e => e.EnvironmentImpactLevel).HasColumnName("environment_impact_level").HasPrecision(10, 4);
        builder.Property(e => e.EnvironmentImpactLevelTags).HasColumnName("environment_impact_level_tags").HasColumnType("jsonb");

        builder.Property(e => e.PackagingRecycling).HasColumnName("packaging_recycling").HasMaxLength(500);
        builder.Property(e => e.PackagingComponents).HasColumnName("packaging_components").HasColumnType("jsonb");
        builder.Property(e => e.PackagingMaterials).HasColumnName("packaging_materials").HasMaxLength(500);
        builder.Property(e => e.PackagingMaterialsTags).HasColumnName("packaging_materials_tags").HasColumnType("jsonb");

        builder.Property(e => e.RecyclingInstruction).HasColumnName("recycling_instruction");
        builder.Property(e => e.RecyclingInstructionToDiscard).HasColumnName("recycling_instruction_to_discard");
        builder.Property(e => e.RecyclingInstructionToRecycle).HasColumnName("recycling_instruction_to_recycle");

        builder.Property(e => e.Origins).HasColumnName("origins").HasMaxLength(500);
        builder.Property(e => e.OriginsTags).HasColumnName("origins_tags").HasColumnType("jsonb");
        builder.Property(e => e.ManufacturingPlaces).HasColumnName("manufacturing_places").HasMaxLength(500);
        builder.Property(e => e.ManufacturingPlacesTags).HasColumnName("manufacturing_places_tags").HasColumnType("jsonb");

        builder.Property(e => e.WaterFootprint100g).HasColumnName("water_footprint_100g").HasPrecision(12, 4);
        builder.Property(e => e.WaterFootprintUnit).HasColumnName("water_footprint_unit").HasMaxLength(50);

        builder.Property(e => e.AgribalyseFoodCode).HasColumnName("agribalyse_food_code");
        builder.Property(e => e.AgribalyseFoodName).HasColumnName("agribalyse_food_name").HasMaxLength(500);
        builder.Property(e => e.AgribalyseCo2Agriculture).HasColumnName("agribalyse_co2_agriculture").HasPrecision(12, 6);
        builder.Property(e => e.AgribalyseCo2Consumption).HasColumnName("agribalyse_co2_consumption").HasPrecision(12, 6);
        builder.Property(e => e.AgribalyseCo2Distribution).HasColumnName("agribalyse_co2_distribution").HasPrecision(12, 6);
        builder.Property(e => e.AgribalyseCo2Packaging).HasColumnName("agribalyse_co2_packaging").HasPrecision(12, 6);
        builder.Property(e => e.AgribalyseCo2Processing).HasColumnName("agribalyse_co2_processing").HasPrecision(12, 6);
        builder.Property(e => e.AgribalyseCo2Transportation).HasColumnName("agribalyse_co2_transportation").HasPrecision(12, 6);
        builder.Property(e => e.AgribalyseCo2Total).HasColumnName("agribalyse_co2_total").HasPrecision(12, 6);
        builder.Property(e => e.AgribalyseEfSingleScore).HasColumnName("agribalyse_ef_single_score").HasPrecision(12, 6);

        builder.Property(e => e.IsForestFootprintFree).HasColumnName("is_forest_footprint_free");
        builder.Property(e => e.ForestFootprint).HasColumnName("forest_footprint").HasPrecision(12, 6);

        builder.Property(e => e.LastSyncedAt).HasColumnName("last_synced_at").HasDefaultValueSql("NOW()");

        builder.HasIndex(e => e.CarbonFootprint100g);

        builder.HasOne(e => e.Product)
            .WithOne(p => p.Environment)
            .HasForeignKey<OffEnvironment>(e => e.Barcode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
