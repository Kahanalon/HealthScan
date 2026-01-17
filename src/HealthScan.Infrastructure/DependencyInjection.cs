using HealthScan.Application.Scoring;
using HealthScan.Application.Services;
using HealthScan.Domain.Interfaces;
using HealthScan.Infrastructure.Caching;
using HealthScan.Infrastructure.ExternalServices;
using HealthScan.Infrastructure.Persistence;
using HealthScan.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthScan.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddMemoryCache();

        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<IContributionRepository, EfContributionRepository>();
        services.AddScoped<IScanEventRepository, EfScanEventRepository>();
        services.AddScoped<IScoringRuleRepository, EfScoringRuleRepository>();

        services.AddSingleton<ICacheService, MemoryCacheService>();

        services.AddHttpClient<IProductDataSource, OpenFoodFactsAdapter>();

        services.AddSingleton<IOcrService, StubOcrService>();

        services.AddSingleton<IIngredientAnalyzer, RegexIngredientAnalyzer>();
        services.AddSingleton<INutritionParser, NutritionTextParser>();
        services.AddScoped<IScoringEngine, CustomScoringEngine>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOcrOrchestrator, OcrOrchestrator>();
        services.AddScoped<IImportService, ImportService>();

        return services;
    }
}
