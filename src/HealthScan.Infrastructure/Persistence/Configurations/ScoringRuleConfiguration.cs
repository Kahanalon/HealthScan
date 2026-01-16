using HealthScan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthScan.Infrastructure.Persistence.Configurations;

public class ScoringRuleConfiguration : IEntityTypeConfiguration<ScoringRule>
{
    public void Configure(EntityTypeBuilder<ScoringRule> builder)
    {
        builder.ToTable("scoring_rules");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");

        builder.Property(r => r.RuleName).HasColumnName("rule_name").HasMaxLength(100).IsRequired();
        builder.HasIndex(r => r.RuleName).IsUnique();

        builder.Property(r => r.RuleType).HasColumnName("rule_type").HasMaxLength(50).IsRequired();
        builder.Property(r => r.ConditionJson).HasColumnName("condition_json").HasColumnType("jsonb").IsRequired();
        builder.Property(r => r.Points).HasColumnName("points").IsRequired();
        builder.Property(r => r.DescriptionHe).HasColumnName("description_he").HasMaxLength(500);
        builder.Property(r => r.DescriptionEn).HasColumnName("description_en").HasMaxLength(500);
        builder.Property(r => r.Priority).HasColumnName("priority").HasDefaultValue(0);
        builder.Property(r => r.IsActive).HasColumnName("is_active").HasDefaultValue(true);
    }
}
