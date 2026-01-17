using HealthScan.Domain.Entities.OpenFoodFacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations.OpenFoodFacts;

public class OffNutritionConfiguration : IEntityTypeConfiguration<OffNutrition>
{
    public void Configure(EntityTypeBuilder<OffNutrition> builder)
    {
        builder.ToTable("off_nutrition");

        builder.HasKey(n => n.Barcode);
        builder.Property(n => n.Barcode).HasColumnName("barcode").HasMaxLength(50).IsRequired();

        builder.Property(n => n.EnergyKcal100g).HasColumnName("energy_kcal_100g").HasPrecision(10, 2);
        builder.Property(n => n.EnergyKj100g).HasColumnName("energy_kj_100g").HasPrecision(10, 2);
        builder.Property(n => n.EnergyKcalServing).HasColumnName("energy_kcal_serving").HasPrecision(10, 2);
        builder.Property(n => n.EnergyKjServing).HasColumnName("energy_kj_serving").HasPrecision(10, 2);

        builder.Property(n => n.Fat100g).HasColumnName("fat_100g").HasPrecision(10, 4);
        builder.Property(n => n.FatServing).HasColumnName("fat_serving").HasPrecision(10, 4);
        builder.Property(n => n.SaturatedFat100g).HasColumnName("saturated_fat_100g").HasPrecision(10, 4);
        builder.Property(n => n.SaturatedFatServing).HasColumnName("saturated_fat_serving").HasPrecision(10, 4);
        builder.Property(n => n.MonounsaturatedFat100g).HasColumnName("monounsaturated_fat_100g").HasPrecision(10, 4);
        builder.Property(n => n.PolyunsaturatedFat100g).HasColumnName("polyunsaturated_fat_100g").HasPrecision(10, 4);
        builder.Property(n => n.TransFat100g).HasColumnName("trans_fat_100g").HasPrecision(10, 4);
        builder.Property(n => n.Cholesterol100g).HasColumnName("cholesterol_100g").HasPrecision(10, 4);
        builder.Property(n => n.Omega3Fat100g).HasColumnName("omega3_fat_100g").HasPrecision(10, 4);
        builder.Property(n => n.Omega6Fat100g).HasColumnName("omega6_fat_100g").HasPrecision(10, 4);

        builder.Property(n => n.Carbohydrates100g).HasColumnName("carbohydrates_100g").HasPrecision(10, 4);
        builder.Property(n => n.CarbohydratesServing).HasColumnName("carbohydrates_serving").HasPrecision(10, 4);
        builder.Property(n => n.Sugars100g).HasColumnName("sugars_100g").HasPrecision(10, 4);
        builder.Property(n => n.SugarsServing).HasColumnName("sugars_serving").HasPrecision(10, 4);
        builder.Property(n => n.Starch100g).HasColumnName("starch_100g").HasPrecision(10, 4);
        builder.Property(n => n.Polyols100g).HasColumnName("polyols_100g").HasPrecision(10, 4);

        builder.Property(n => n.Fiber100g).HasColumnName("fiber_100g").HasPrecision(10, 4);
        builder.Property(n => n.FiberServing).HasColumnName("fiber_serving").HasPrecision(10, 4);

        builder.Property(n => n.Proteins100g).HasColumnName("proteins_100g").HasPrecision(10, 4);
        builder.Property(n => n.ProteinsServing).HasColumnName("proteins_serving").HasPrecision(10, 4);

        builder.Property(n => n.Salt100g).HasColumnName("salt_100g").HasPrecision(10, 4);
        builder.Property(n => n.SaltServing).HasColumnName("salt_serving").HasPrecision(10, 4);
        builder.Property(n => n.Sodium100g).HasColumnName("sodium_100g").HasPrecision(10, 4);
        builder.Property(n => n.SodiumServing).HasColumnName("sodium_serving").HasPrecision(10, 4);

        builder.Property(n => n.VitaminA100g).HasColumnName("vitamin_a_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminB1100g).HasColumnName("vitamin_b1_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminB2100g).HasColumnName("vitamin_b2_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminB6100g).HasColumnName("vitamin_b6_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminB9100g).HasColumnName("vitamin_b9_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminB12100g).HasColumnName("vitamin_b12_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminC100g).HasColumnName("vitamin_c_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminD100g).HasColumnName("vitamin_d_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminE100g).HasColumnName("vitamin_e_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminK100g).HasColumnName("vitamin_k_100g").HasPrecision(12, 6);
        builder.Property(n => n.VitaminPp100g).HasColumnName("vitamin_pp_100g").HasPrecision(12, 6);

        builder.Property(n => n.Calcium100g).HasColumnName("calcium_100g").HasPrecision(12, 6);
        builder.Property(n => n.Iron100g).HasColumnName("iron_100g").HasPrecision(12, 6);
        builder.Property(n => n.Magnesium100g).HasColumnName("magnesium_100g").HasPrecision(12, 6);
        builder.Property(n => n.Zinc100g).HasColumnName("zinc_100g").HasPrecision(12, 6);
        builder.Property(n => n.Phosphorus100g).HasColumnName("phosphorus_100g").HasPrecision(12, 6);
        builder.Property(n => n.Potassium100g).HasColumnName("potassium_100g").HasPrecision(12, 6);
        builder.Property(n => n.Iodine100g).HasColumnName("iodine_100g").HasPrecision(12, 6);
        builder.Property(n => n.Selenium100g).HasColumnName("selenium_100g").HasPrecision(12, 6);
        builder.Property(n => n.Copper100g).HasColumnName("copper_100g").HasPrecision(12, 6);
        builder.Property(n => n.Manganese100g).HasColumnName("manganese_100g").HasPrecision(12, 6);
        builder.Property(n => n.Fluoride100g).HasColumnName("fluoride_100g").HasPrecision(12, 6);

        builder.Property(n => n.Caffeine100g).HasColumnName("caffeine_100g").HasPrecision(10, 4);
        builder.Property(n => n.Taurine100g).HasColumnName("taurine_100g").HasPrecision(10, 4);
        builder.Property(n => n.Alcohol100g).HasColumnName("alcohol_100g").HasPrecision(10, 4);

        builder.Property(n => n.NutritionDataPer).HasColumnName("nutrition_data_per").HasMaxLength(50);
        builder.Property(n => n.NutritionGradeFr).HasColumnName("nutrition_grade_fr").HasMaxLength(50);

        builder.Property(n => n.LastSyncedAt).HasColumnName("last_synced_at").HasDefaultValueSql("NOW()");

        builder.HasOne(n => n.Product)
            .WithOne(p => p.Nutrition)
            .HasForeignKey<OffNutrition>(n => n.Barcode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
