using HealthScan.Domain.Entities.OpenFoodFacts;
using HealthScan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthScan.Infrastructure.Persistence.Repositories;

public class EfOffProductRepository : IOffProductRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<EfOffProductRepository> _logger;

    public EfOffProductRepository(AppDbContext context, ILogger<EfOffProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OffProduct?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return await _context.OffProducts
            .Include(p => p.Nutrition)
            .Include(p => p.Ingredients)
            .Include(p => p.Scores)
            .Include(p => p.Images)
            .Include(p => p.Environment)
            .FirstOrDefaultAsync(p => p.Barcode == barcode, cancellationToken);
    }

    public async Task<List<OffProduct>> GetAllAsync(int skip = 0, int take = 100, CancellationToken cancellationToken = default)
    {
        return await _context.OffProducts
            .Include(p => p.Nutrition)
            .Include(p => p.Ingredients)
            .Include(p => p.Scores)
            .Include(p => p.Images)
            .Include(p => p.Environment)
            .OrderByDescending(p => p.LastSyncedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OffProducts.CountAsync(cancellationToken);
    }

    public async Task<int> BulkUpsertAsync(List<OffProduct> products, CancellationToken cancellationToken = default)
    {
        if (products.Count == 0)
            return 0;

        var upsertedCount = 0;

        foreach (var product in products)
        {
            try
            {
                var existing = await _context.OffProducts
                    .Include(p => p.Nutrition)
                    .Include(p => p.Ingredients)
                    .Include(p => p.Scores)
                    .Include(p => p.Images)
                    .Include(p => p.Environment)
                    .FirstOrDefaultAsync(p => p.Barcode == product.Barcode, cancellationToken);

                if (existing != null)
                {
                    UpdateExistingProduct(existing, product);
                }
                else
                {
                    _context.OffProducts.Add(product);
                }

                upsertedCount++;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to upsert OFF product {Barcode}", product.Barcode);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return upsertedCount;
    }

    public async Task<bool> UpsertAsync(OffProduct product, CancellationToken cancellationToken = default)
    {
        var existing = await _context.OffProducts
            .Include(p => p.Nutrition)
            .Include(p => p.Ingredients)
            .Include(p => p.Scores)
            .Include(p => p.Images)
            .Include(p => p.Environment)
            .FirstOrDefaultAsync(p => p.Barcode == product.Barcode, cancellationToken);

        if (existing != null)
        {
            UpdateExistingProduct(existing, product);
        }
        else
        {
            _context.OffProducts.Add(product);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<OffStats> GetStatsAsync(CancellationToken cancellationToken = default)
    {
        var total = await _context.OffProducts.CountAsync(cancellationToken);

        var withNutriScore = await _context.OffScores
            .Where(s => s.NutriScoreGrade != null)
            .CountAsync(cancellationToken);

        var withEcoScore = await _context.OffScores
            .Where(s => s.EcoScoreGrade != null)
            .CountAsync(cancellationToken);

        var withNovaGroup = await _context.OffScores
            .Where(s => s.NovaGroup != null)
            .CountAsync(cancellationToken);

        var withIngredients = await _context.OffIngredients
            .Where(i => i.IngredientsText != null || i.IngredientsTextEn != null || i.IngredientsTextHe != null)
            .CountAsync(cancellationToken);

        var withAllergens = await _context.OffIngredients
            .Where(i => i.AllergensTags != null && i.AllergensTags.Count > 0)
            .CountAsync(cancellationToken);

        var withImages = await _context.OffImages
            .Where(i => i.ImageFrontUrl != null)
            .CountAsync(cancellationToken);

        var lastSynced = await _context.OffProducts
            .OrderByDescending(p => p.LastSyncedAt)
            .Select(p => p.LastSyncedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var nutriScoreDistribution = await _context.OffScores
            .Where(s => s.NutriScoreGrade != null)
            .GroupBy(s => s.NutriScoreGrade!)
            .Select(g => new { Grade = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Grade, x => x.Count, cancellationToken);

        var novaGroupDistribution = await _context.OffScores
            .Where(s => s.NovaGroup != null)
            .GroupBy(s => s.NovaGroup!.Value)
            .Select(g => new { Group = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Group, x => x.Count, cancellationToken);

        return new OffStats
        {
            TotalProducts = total,
            WithNutriScore = withNutriScore,
            WithEcoScore = withEcoScore,
            WithNovaGroup = withNovaGroup,
            WithIngredients = withIngredients,
            WithAllergens = withAllergens,
            WithImages = withImages,
            LastSyncedAt = lastSynced,
            NutriScoreDistribution = nutriScoreDistribution,
            NovaGroupDistribution = novaGroupDistribution
        };
    }

    private void UpdateExistingProduct(OffProduct existing, OffProduct updated)
    {
        existing.ProductName = updated.ProductName;
        existing.ProductNameHe = updated.ProductNameHe;
        existing.ProductNameEn = updated.ProductNameEn;
        existing.GenericName = updated.GenericName;
        existing.GenericNameHe = updated.GenericNameHe;
        existing.GenericNameEn = updated.GenericNameEn;
        existing.Brands = updated.Brands;
        existing.BrandsTags = updated.BrandsTags;
        existing.Quantity = updated.Quantity;
        existing.ServingSize = updated.ServingSize;
        existing.ServingQuantity = updated.ServingQuantity;
        existing.Categories = updated.Categories;
        existing.CategoriesTags = updated.CategoriesTags;
        existing.CategoriesHierarchy = updated.CategoriesHierarchy;
        existing.Labels = updated.Labels;
        existing.LabelsTags = updated.LabelsTags;
        existing.Stores = updated.Stores;
        existing.Countries = updated.Countries;
        existing.CountriesTags = updated.CountriesTags;
        existing.ManufacturingPlaces = updated.ManufacturingPlaces;
        existing.Origins = updated.Origins;
        existing.Packaging = updated.Packaging;
        existing.PackagingTags = updated.PackagingTags;
        existing.Completeness = updated.Completeness;
        existing.LastModifiedT = updated.LastModifiedT;
        existing.Editor = updated.Editor;
        existing.EditorsCount = updated.EditorsCount;
        existing.States = updated.States;
        existing.StatesTags = updated.StatesTags;
        existing.LastSyncedAt = DateTime.UtcNow;

        if (existing.Nutrition != null && updated.Nutrition != null)
        {
            UpdateNutrition(existing.Nutrition, updated.Nutrition);
        }
        else if (updated.Nutrition != null)
        {
            existing.Nutrition = updated.Nutrition;
        }

        if (existing.Ingredients != null && updated.Ingredients != null)
        {
            UpdateIngredients(existing.Ingredients, updated.Ingredients);
        }
        else if (updated.Ingredients != null)
        {
            existing.Ingredients = updated.Ingredients;
        }

        if (existing.Scores != null && updated.Scores != null)
        {
            UpdateScores(existing.Scores, updated.Scores);
        }
        else if (updated.Scores != null)
        {
            existing.Scores = updated.Scores;
        }

        if (existing.Images != null && updated.Images != null)
        {
            UpdateImages(existing.Images, updated.Images);
        }
        else if (updated.Images != null)
        {
            existing.Images = updated.Images;
        }

        if (existing.Environment != null && updated.Environment != null)
        {
            UpdateEnvironment(existing.Environment, updated.Environment);
        }
        else if (updated.Environment != null)
        {
            existing.Environment = updated.Environment;
        }
    }

    private static void UpdateNutrition(OffNutrition existing, OffNutrition updated)
    {
        existing.EnergyKcal100g = updated.EnergyKcal100g;
        existing.EnergyKj100g = updated.EnergyKj100g;
        existing.EnergyKcalServing = updated.EnergyKcalServing;
        existing.EnergyKjServing = updated.EnergyKjServing;
        existing.Fat100g = updated.Fat100g;
        existing.FatServing = updated.FatServing;
        existing.SaturatedFat100g = updated.SaturatedFat100g;
        existing.SaturatedFatServing = updated.SaturatedFatServing;
        existing.MonounsaturatedFat100g = updated.MonounsaturatedFat100g;
        existing.PolyunsaturatedFat100g = updated.PolyunsaturatedFat100g;
        existing.TransFat100g = updated.TransFat100g;
        existing.Cholesterol100g = updated.Cholesterol100g;
        existing.Omega3Fat100g = updated.Omega3Fat100g;
        existing.Omega6Fat100g = updated.Omega6Fat100g;
        existing.Carbohydrates100g = updated.Carbohydrates100g;
        existing.CarbohydratesServing = updated.CarbohydratesServing;
        existing.Sugars100g = updated.Sugars100g;
        existing.SugarsServing = updated.SugarsServing;
        existing.Starch100g = updated.Starch100g;
        existing.Polyols100g = updated.Polyols100g;
        existing.Fiber100g = updated.Fiber100g;
        existing.FiberServing = updated.FiberServing;
        existing.Proteins100g = updated.Proteins100g;
        existing.ProteinsServing = updated.ProteinsServing;
        existing.Salt100g = updated.Salt100g;
        existing.SaltServing = updated.SaltServing;
        existing.Sodium100g = updated.Sodium100g;
        existing.SodiumServing = updated.SodiumServing;
        existing.VitaminA100g = updated.VitaminA100g;
        existing.VitaminB1100g = updated.VitaminB1100g;
        existing.VitaminB2100g = updated.VitaminB2100g;
        existing.VitaminB6100g = updated.VitaminB6100g;
        existing.VitaminB9100g = updated.VitaminB9100g;
        existing.VitaminB12100g = updated.VitaminB12100g;
        existing.VitaminC100g = updated.VitaminC100g;
        existing.VitaminD100g = updated.VitaminD100g;
        existing.VitaminE100g = updated.VitaminE100g;
        existing.VitaminK100g = updated.VitaminK100g;
        existing.VitaminPp100g = updated.VitaminPp100g;
        existing.Calcium100g = updated.Calcium100g;
        existing.Iron100g = updated.Iron100g;
        existing.Magnesium100g = updated.Magnesium100g;
        existing.Zinc100g = updated.Zinc100g;
        existing.Phosphorus100g = updated.Phosphorus100g;
        existing.Potassium100g = updated.Potassium100g;
        existing.Iodine100g = updated.Iodine100g;
        existing.Selenium100g = updated.Selenium100g;
        existing.Copper100g = updated.Copper100g;
        existing.Manganese100g = updated.Manganese100g;
        existing.Fluoride100g = updated.Fluoride100g;
        existing.Caffeine100g = updated.Caffeine100g;
        existing.Taurine100g = updated.Taurine100g;
        existing.Alcohol100g = updated.Alcohol100g;
        existing.NutritionDataPer = updated.NutritionDataPer;
        existing.NutritionGradeFr = updated.NutritionGradeFr;
        existing.LastSyncedAt = DateTime.UtcNow;
    }

    private static void UpdateIngredients(OffIngredients existing, OffIngredients updated)
    {
        existing.IngredientsText = updated.IngredientsText;
        existing.IngredientsTextHe = updated.IngredientsTextHe;
        existing.IngredientsTextEn = updated.IngredientsTextEn;
        existing.IngredientsParsed = updated.IngredientsParsed;
        existing.IngredientsCount = updated.IngredientsCount;
        existing.IngredientsPercentAnalysis = updated.IngredientsPercentAnalysis;
        existing.Allergens = updated.Allergens;
        existing.AllergensTags = updated.AllergensTags;
        existing.AllergensHierarchy = updated.AllergensHierarchy;
        existing.Traces = updated.Traces;
        existing.TracesTags = updated.TracesTags;
        existing.Additives = updated.Additives;
        existing.AdditivesTags = updated.AdditivesTags;
        existing.AdditivesCount = updated.AdditivesCount;
        existing.AminoAcidsTags = updated.AminoAcidsTags;
        existing.MineralsTags = updated.MineralsTags;
        existing.VitaminsTags = updated.VitaminsTags;
        existing.NucleotidesTags = updated.NucleotidesTags;
        existing.OtherNutritionalSubstancesTags = updated.OtherNutritionalSubstancesTags;
        existing.NovaGroup = updated.NovaGroup;
        existing.NovaGroupsMarkers = updated.NovaGroupsMarkers;
        existing.NovaGroupsTags = updated.NovaGroupsTags;
        existing.IsPalmOilFree = updated.IsPalmOilFree;
        existing.IsVegan = updated.IsVegan;
        existing.IsVegetarian = updated.IsVegetarian;
        existing.VeganAnalysis = updated.VeganAnalysis;
        existing.VegetarianAnalysis = updated.VegetarianAnalysis;
        existing.IngredientsAnalysis = updated.IngredientsAnalysis;
        existing.IngredientsAnalysisTags = updated.IngredientsAnalysisTags;
        existing.LastSyncedAt = DateTime.UtcNow;
    }

    private static void UpdateScores(OffScores existing, OffScores updated)
    {
        existing.NutriScoreGrade = updated.NutriScoreGrade;
        existing.NutriScoreScore = updated.NutriScoreScore;
        existing.NutriScoreVersion = updated.NutriScoreVersion;
        existing.NutriscoreGrade2021 = updated.NutriscoreGrade2021;
        existing.NutriscoreScore2021 = updated.NutriscoreScore2021;
        existing.NutriscoreNegativePoints2021 = updated.NutriscoreNegativePoints2021;
        existing.NutriscorePositivePoints2021 = updated.NutriscorePositivePoints2021;
        existing.NutriscoreGrade2023 = updated.NutriscoreGrade2023;
        existing.NutriscoreScore2023 = updated.NutriscoreScore2023;
        existing.NutriscoreNegativePoints2023 = updated.NutriscoreNegativePoints2023;
        existing.NutriscorePositivePoints2023 = updated.NutriscorePositivePoints2023;
        existing.EcoScoreGrade = updated.EcoScoreGrade;
        existing.EcoScoreScore = updated.EcoScoreScore;
        existing.EcoScoreVersion = updated.EcoScoreVersion;
        existing.EcoScoreAdjustments = updated.EcoScoreAdjustments;
        existing.EcoScorePackaging = updated.EcoScorePackaging;
        existing.EcoScoreProduction = updated.EcoScoreProduction;
        existing.EcoScoreOrigins = updated.EcoScoreOrigins;
        existing.EcoScoreThreatenedSpecies = updated.EcoScoreThreatenedSpecies;
        existing.NovaGroup = updated.NovaGroup;
        existing.NutrientLevelsEnergy = updated.NutrientLevelsEnergy;
        existing.NutrientLevelsFat = updated.NutrientLevelsFat;
        existing.NutrientLevelsSaturatedFat = updated.NutrientLevelsSaturatedFat;
        existing.NutrientLevelsSugars = updated.NutrientLevelsSugars;
        existing.NutrientLevelsSalt = updated.NutrientLevelsSalt;
        existing.DataQualityErrorsCount = updated.DataQualityErrorsCount;
        existing.DataQualityErrorsTags = updated.DataQualityErrorsTags;
        existing.DataQualityWarningsCount = updated.DataQualityWarningsCount;
        existing.DataQualityWarningsTags = updated.DataQualityWarningsTags;
        existing.DataQualityInfoCount = updated.DataQualityInfoCount;
        existing.DataQualityInfoTags = updated.DataQualityInfoTags;
        existing.UnknownNutrientsCount = updated.UnknownNutrientsCount;
        existing.KnownNutrientsCount = updated.KnownNutrientsCount;
        existing.LastSyncedAt = DateTime.UtcNow;
    }

    private static void UpdateImages(OffImages existing, OffImages updated)
    {
        existing.ImageUrl = updated.ImageUrl;
        existing.ImageSmallUrl = updated.ImageSmallUrl;
        existing.ImageThumbUrl = updated.ImageThumbUrl;
        existing.ImageFrontUrl = updated.ImageFrontUrl;
        existing.ImageFrontSmallUrl = updated.ImageFrontSmallUrl;
        existing.ImageFrontThumbUrl = updated.ImageFrontThumbUrl;
        existing.ImageNutritionUrl = updated.ImageNutritionUrl;
        existing.ImageNutritionSmallUrl = updated.ImageNutritionSmallUrl;
        existing.ImageNutritionThumbUrl = updated.ImageNutritionThumbUrl;
        existing.ImageIngredientsUrl = updated.ImageIngredientsUrl;
        existing.ImageIngredientsSmallUrl = updated.ImageIngredientsSmallUrl;
        existing.ImageIngredientsThumbUrl = updated.ImageIngredientsThumbUrl;
        existing.ImagePackagingUrl = updated.ImagePackagingUrl;
        existing.ImagePackagingSmallUrl = updated.ImagePackagingSmallUrl;
        existing.ImagePackagingThumbUrl = updated.ImagePackagingThumbUrl;
        existing.SelectedImages = updated.SelectedImages;
        existing.ImagesKeys = updated.ImagesKeys;
        existing.ImagesCount = updated.ImagesCount;
        existing.LastSyncedAt = DateTime.UtcNow;
    }

    private static void UpdateEnvironment(OffEnvironment existing, OffEnvironment updated)
    {
        existing.CarbonFootprint100g = updated.CarbonFootprint100g;
        existing.CarbonFootprintServing = updated.CarbonFootprintServing;
        existing.CarbonFootprintUnit = updated.CarbonFootprintUnit;
        existing.CarbonFootprintSource = updated.CarbonFootprintSource;
        existing.EnvironmentImpactLevel = updated.EnvironmentImpactLevel;
        existing.EnvironmentImpactLevelTags = updated.EnvironmentImpactLevelTags;
        existing.PackagingRecycling = updated.PackagingRecycling;
        existing.PackagingComponents = updated.PackagingComponents;
        existing.PackagingMaterials = updated.PackagingMaterials;
        existing.PackagingMaterialsTags = updated.PackagingMaterialsTags;
        existing.RecyclingInstruction = updated.RecyclingInstruction;
        existing.RecyclingInstructionToDiscard = updated.RecyclingInstructionToDiscard;
        existing.RecyclingInstructionToRecycle = updated.RecyclingInstructionToRecycle;
        existing.Origins = updated.Origins;
        existing.OriginsTags = updated.OriginsTags;
        existing.ManufacturingPlaces = updated.ManufacturingPlaces;
        existing.ManufacturingPlacesTags = updated.ManufacturingPlacesTags;
        existing.WaterFootprint100g = updated.WaterFootprint100g;
        existing.WaterFootprintUnit = updated.WaterFootprintUnit;
        existing.AgribalyseFoodCode = updated.AgribalyseFoodCode;
        existing.AgribalyseFoodName = updated.AgribalyseFoodName;
        existing.AgribalyseCo2Agriculture = updated.AgribalyseCo2Agriculture;
        existing.AgribalyseCo2Consumption = updated.AgribalyseCo2Consumption;
        existing.AgribalyseCo2Distribution = updated.AgribalyseCo2Distribution;
        existing.AgribalyseCo2Packaging = updated.AgribalyseCo2Packaging;
        existing.AgribalyseCo2Processing = updated.AgribalyseCo2Processing;
        existing.AgribalyseCo2Transportation = updated.AgribalyseCo2Transportation;
        existing.AgribalyseCo2Total = updated.AgribalyseCo2Total;
        existing.AgribalyseEfSingleScore = updated.AgribalyseEfSingleScore;
        existing.IsForestFootprintFree = updated.IsForestFootprintFree;
        existing.ForestFootprint = updated.ForestFootprint;
        existing.LastSyncedAt = DateTime.UtcNow;
    }
}
