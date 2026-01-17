using HealthScan.Domain.Entities.OpenFoodFacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations.OpenFoodFacts;

public class OffIngredientsConfiguration : IEntityTypeConfiguration<OffIngredients>
{
    public void Configure(EntityTypeBuilder<OffIngredients> builder)
    {
        builder.ToTable("off_ingredients");

        builder.HasKey(i => i.Barcode);
        builder.Property(i => i.Barcode).HasColumnName("barcode").HasMaxLength(50).IsRequired();

        builder.Property(i => i.IngredientsText).HasColumnName("ingredients_text");
        builder.Property(i => i.IngredientsTextHe).HasColumnName("ingredients_text_he");
        builder.Property(i => i.IngredientsTextEn).HasColumnName("ingredients_text_en");

        builder.Property(i => i.IngredientsParsed).HasColumnName("ingredients_parsed").HasColumnType("jsonb");
        builder.Property(i => i.IngredientsCount).HasColumnName("ingredients_count");
        builder.Property(i => i.IngredientsPercentAnalysis).HasColumnName("ingredients_percent_analysis").HasPrecision(5, 2);

        builder.Property(i => i.Allergens).HasColumnName("allergens").HasColumnType("jsonb");
        builder.Property(i => i.AllergensTags).HasColumnName("allergens_tags").HasColumnType("jsonb");
        builder.Property(i => i.AllergensHierarchy).HasColumnName("allergens_hierarchy");

        builder.Property(i => i.Traces).HasColumnName("traces").HasColumnType("jsonb");
        builder.Property(i => i.TracesTags).HasColumnName("traces_tags").HasColumnType("jsonb");

        builder.Property(i => i.Additives).HasColumnName("additives").HasColumnType("jsonb");
        builder.Property(i => i.AdditivesTags).HasColumnName("additives_tags").HasColumnType("jsonb");
        builder.Property(i => i.AdditivesCount).HasColumnName("additives_count");

        builder.Property(i => i.AminoAcidsTags).HasColumnName("amino_acids_tags").HasColumnType("jsonb");
        builder.Property(i => i.MineralsTags).HasColumnName("minerals_tags").HasColumnType("jsonb");
        builder.Property(i => i.VitaminsTags).HasColumnName("vitamins_tags").HasColumnType("jsonb");
        builder.Property(i => i.NucleotidesTags).HasColumnName("nucleotides_tags").HasColumnType("jsonb");
        builder.Property(i => i.OtherNutritionalSubstancesTags).HasColumnName("other_nutritional_substances_tags").HasColumnType("jsonb");

        builder.Property(i => i.NovaGroup).HasColumnName("nova_group");
        builder.Property(i => i.NovaGroupsMarkers).HasColumnName("nova_groups_markers");
        builder.Property(i => i.NovaGroupsTags).HasColumnName("nova_groups_tags").HasColumnType("jsonb");

        builder.Property(i => i.IsPalmOilFree).HasColumnName("is_palm_oil_free");
        builder.Property(i => i.IsVegan).HasColumnName("is_vegan");
        builder.Property(i => i.IsVegetarian).HasColumnName("is_vegetarian");
        builder.Property(i => i.VeganAnalysis).HasColumnName("vegan_analysis").HasMaxLength(50);
        builder.Property(i => i.VegetarianAnalysis).HasColumnName("vegetarian_analysis").HasMaxLength(50);

        builder.Property(i => i.IngredientsAnalysis).HasColumnName("ingredients_analysis");
        builder.Property(i => i.IngredientsAnalysisTags).HasColumnName("ingredients_analysis_tags").HasColumnType("jsonb");

        builder.Property(i => i.LastSyncedAt).HasColumnName("last_synced_at").HasDefaultValueSql("NOW()");

        builder.HasIndex(i => i.NovaGroup);
        builder.HasIndex(i => i.IsVegan);
        builder.HasIndex(i => i.IsVegetarian);

        builder.HasOne(i => i.Product)
            .WithOne(p => p.Ingredients)
            .HasForeignKey<OffIngredients>(i => i.Barcode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
