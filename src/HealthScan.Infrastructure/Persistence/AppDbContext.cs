using HealthScan.Domain.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
