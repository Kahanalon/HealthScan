using HealthScan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations;

public class IngredientFlagConfiguration : IEntityTypeConfiguration<IngredientFlag>
{
    public void Configure(EntityTypeBuilder<IngredientFlag> builder)
    {
        builder.ToTable("ingredient_flags");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id");

        builder.Property(f => f.IngredientPattern).HasColumnName("ingredient_pattern").HasMaxLength(200).IsRequired();
        builder.Property(f => f.FlagType).HasColumnName("flag_type").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.HasIndex(f => f.FlagType);

        builder.Property(f => f.PenaltyPoints).HasColumnName("penalty_points").HasDefaultValue(0);
        builder.Property(f => f.DescriptionHe).HasColumnName("description_he").HasMaxLength(500);
        builder.Property(f => f.DescriptionEn).HasColumnName("description_en").HasMaxLength(500);
        builder.Property(f => f.IsActive).HasColumnName("is_active").HasDefaultValue(true);
    }
}
