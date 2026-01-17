using HealthScan.Domain.Entities.OpenFoodFacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations.OpenFoodFacts;

public class OffScoresConfiguration : IEntityTypeConfiguration<OffScores>
{
    public void Configure(EntityTypeBuilder<OffScores> builder)
    {
        builder.ToTable("off_scores");

        builder.HasKey(s => s.Barcode);
        builder.Property(s => s.Barcode).HasColumnName("barcode").HasMaxLength(50).IsRequired();

        builder.Property(s => s.NutriScoreGrade).HasColumnName("nutri_score_grade").HasMaxLength(20);
        builder.Property(s => s.NutriScoreScore).HasColumnName("nutri_score_score");
        builder.Property(s => s.NutriScoreVersion).HasColumnName("nutri_score_version").HasMaxLength(20);

        builder.Property(s => s.NutriscoreGrade2021).HasColumnName("nutriscore_grade_2021").HasMaxLength(20);
        builder.Property(s => s.NutriscoreScore2021).HasColumnName("nutriscore_score_2021");
        builder.Property(s => s.NutriscoreNegativePoints2021).HasColumnName("nutriscore_negative_points_2021");
        builder.Property(s => s.NutriscorePositivePoints2021).HasColumnName("nutriscore_positive_points_2021");

        builder.Property(s => s.NutriscoreGrade2023).HasColumnName("nutriscore_grade_2023").HasMaxLength(20);
        builder.Property(s => s.NutriscoreScore2023).HasColumnName("nutriscore_score_2023");
        builder.Property(s => s.NutriscoreNegativePoints2023).HasColumnName("nutriscore_negative_points_2023");
        builder.Property(s => s.NutriscorePositivePoints2023).HasColumnName("nutriscore_positive_points_2023");

        builder.Property(s => s.EcoScoreGrade).HasColumnName("eco_score_grade").HasMaxLength(20);
        builder.Property(s => s.EcoScoreScore).HasColumnName("eco_score_score");
        builder.Property(s => s.EcoScoreVersion).HasColumnName("eco_score_version").HasMaxLength(20);

        builder.Property(s => s.EcoScoreAdjustments).HasColumnName("eco_score_adjustments");
        builder.Property(s => s.EcoScorePackaging).HasColumnName("eco_score_packaging");
        builder.Property(s => s.EcoScoreProduction).HasColumnName("eco_score_production");
        builder.Property(s => s.EcoScoreOrigins).HasColumnName("eco_score_origins");
        builder.Property(s => s.EcoScoreThreatenedSpecies).HasColumnName("eco_score_threatened_species");

        builder.Property(s => s.NovaGroup).HasColumnName("nova_group");

        builder.Property(s => s.NutrientLevelsEnergy).HasColumnName("nutrient_levels_energy").HasPrecision(10, 2);
        builder.Property(s => s.NutrientLevelsFat).HasColumnName("nutrient_levels_fat").HasMaxLength(20);
        builder.Property(s => s.NutrientLevelsSaturatedFat).HasColumnName("nutrient_levels_saturated_fat").HasMaxLength(20);
        builder.Property(s => s.NutrientLevelsSugars).HasColumnName("nutrient_levels_sugars").HasMaxLength(20);
        builder.Property(s => s.NutrientLevelsSalt).HasColumnName("nutrient_levels_salt").HasMaxLength(20);

        builder.Property(s => s.DataQualityErrorsCount).HasColumnName("data_quality_errors_count");
        builder.Property(s => s.DataQualityErrorsTags).HasColumnName("data_quality_errors_tags").HasColumnType("jsonb");
        builder.Property(s => s.DataQualityWarningsCount).HasColumnName("data_quality_warnings_count");
        builder.Property(s => s.DataQualityWarningsTags).HasColumnName("data_quality_warnings_tags").HasColumnType("jsonb");
        builder.Property(s => s.DataQualityInfoCount).HasColumnName("data_quality_info_count");
        builder.Property(s => s.DataQualityInfoTags).HasColumnName("data_quality_info_tags").HasColumnType("jsonb");

        builder.Property(s => s.UnknownNutrientsCount).HasColumnName("unknown_nutrients_count").HasPrecision(10, 2);
        builder.Property(s => s.KnownNutrientsCount).HasColumnName("known_nutrients_count").HasPrecision(10, 2);

        builder.Property(s => s.LastSyncedAt).HasColumnName("last_synced_at").HasDefaultValueSql("NOW()");

        builder.HasIndex(s => s.NutriScoreGrade);
        builder.HasIndex(s => s.EcoScoreGrade);
        builder.HasIndex(s => s.NovaGroup);

        builder.HasOne(s => s.Product)
            .WithOne(p => p.Scores)
            .HasForeignKey<OffScores>(s => s.Barcode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
