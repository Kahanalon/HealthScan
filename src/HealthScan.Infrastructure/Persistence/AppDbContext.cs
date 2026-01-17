using HealthScan.Domain.Entities;
using HealthScan.Domain.Entities.OpenFoodFacts;
using Microsoft.EntityFrameworkCore;

namespace HealthScan.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductContribution> ProductContributions => Set<ProductContribution>();
    public DbSet<ScoringRule> ScoringRules => Set<ScoringRule>();
    public DbSet<IngredientFlag> IngredientFlags => Set<IngredientFlag>();
    public DbSet<ScanEvent> ScanEvents => Set<ScanEvent>();

    public DbSet<OffProduct> OffProducts => Set<OffProduct>();
    public DbSet<OffNutrition> OffNutrition => Set<OffNutrition>();
    public DbSet<OffIngredients> OffIngredients => Set<OffIngredients>();
    public DbSet<OffScores> OffScores => Set<OffScores>();
    public DbSet<OffImages> OffImages => Set<OffImages>();
    public DbSet<OffEnvironment> OffEnvironment => Set<OffEnvironment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
