using System.Text.Json;
using System.Text.Json.Serialization;
using HealthScan.Domain.Entities;
using HealthScan.Domain.Entities.OpenFoodFacts;
using HealthScan.Domain.Enums;
using HealthScan.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HealthScan.Infrastructure.ExternalServices;

public class OpenFoodFactsAdapter : IProductDataSource, IOffDataSource
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenFoodFactsAdapter> _logger;
    private const string BaseUrl = "https://world.openfoodfacts.org/api/v2";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals
    };

    public string SourceName => "openfoodfacts";

    public OpenFoodFactsAdapter(HttpClient httpClient, ILogger<OpenFoodFactsAdapter> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "HealthScan/1.0 (contact@healthscan.app)");
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{BaseUrl}/product/{barcode}.json";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Open Food Facts returned {StatusCode} for barcode {Barcode}", response.StatusCode, barcode);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var offResponse = JsonSerializer.Deserialize<OpenFoodFactsResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (offResponse?.Status != 1 || offResponse.Product == null)
            {
                _logger.LogInformation("Product not found in Open Food Facts: {Barcode}", barcode);
                return null;
            }

            return MapToProduct(offResponse.Product, barcode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product from Open Food Facts: {Barcode}", barcode);
            return null;
        }
    }

    public async Task<List<Product>> SearchAsync(string query, int limit = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{BaseUrl}/search?search_terms={Uri.EscapeDataString(query)}&countries_tags=israel&page_size={limit}&json=1";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return new List<Product>();
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var searchResponse = JsonSerializer.Deserialize<OpenFoodFactsSearchResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (searchResponse?.Products == null)
            {
                return new List<Product>();
            }

            return searchResponse.Products
                .Where(p => !string.IsNullOrEmpty(p.Code))
                .Select(p => MapToProduct(p, p.Code!))
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching Open Food Facts: {Query}", query);
            return new List<Product>();
        }
    }

    public async Task<List<Product>> FetchIsraeliProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{BaseUrl}/search?countries_tags=israel&page={page}&page_size={pageSize}&json=1&fields=code,product_name,product_name_he,product_name_en,brands,quantity,categories,ingredients_text,ingredients_text_he,ingredients_text_en,image_front_url,image_nutrition_url,image_ingredients_url,nutriments";
            _logger.LogInformation("Fetching Israeli products page {Page} from Open Food Facts", page);

            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Open Food Facts returned {StatusCode} for Israeli products fetch", response.StatusCode);
                return new List<Product>();
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var searchResponse = JsonSerializer.Deserialize<OpenFoodFactsSearchResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (searchResponse?.Products == null)
            {
                return new List<Product>();
            }

            var products = searchResponse.Products
                .Where(p => !string.IsNullOrEmpty(p.Code))
                .Select(p => MapToProduct(p, p.Code!))
                .ToList();

            _logger.LogInformation("Fetched {Count} Israeli products from page {Page}", products.Count, page);
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Israeli products from Open Food Facts page {Page}", page);
            return new List<Product>();
        }
    }

    public async Task<int> GetIsraeliProductCountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{BaseUrl}/search?countries_tags=israel&page=1&page_size=1&json=1&fields=code";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var searchResponse = JsonSerializer.Deserialize<OpenFoodFactsCountResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return searchResponse?.Count ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Israeli product count from Open Food Facts");
            return 0;
        }
    }

    private Product MapToProduct(OpenFoodFactsProduct offProduct, string barcode)
    {
        var nutriments = offProduct.Nutriments ?? new OpenFoodFactsNutriments();

        return new Product
        {
            Barcode = barcode,
            NameHe = offProduct.ProductNameHe,
            NameEn = offProduct.ProductNameEn ?? offProduct.ProductName,
            Brand = offProduct.Brands,
            PackageSize = offProduct.Quantity,
            Category = offProduct.Categories,
            Energy100g = nutriments.EnergyKcal100g,
            Fat100g = nutriments.Fat100g,
            SaturatedFat100g = nutriments.SaturatedFat100g,
            Carbohydrates100g = nutriments.Carbohydrates100g,
            Sugars100g = nutriments.Sugars100g,
            Fiber100g = nutriments.Fiber100g,
            Protein100g = nutriments.Proteins100g,
            Sodium100g = nutriments.Sodium100g.HasValue ? nutriments.Sodium100g * 1000 : null,
            IngredientsTextHe = offProduct.IngredientsTextHe,
            IngredientsTextEn = offProduct.IngredientsTextEn ?? offProduct.IngredientsText,
            ImageFrontUrl = offProduct.ImageFrontUrl,
            ImageNutritionUrl = offProduct.ImageNutritionUrl,
            ImageIngredientsUrl = offProduct.ImageIngredientsUrl,
            Source = SourceName,
            Status = ProductStatus.Pending,
            NutritionComplete = nutriments.Sugars100g.HasValue &&
                               nutriments.Sodium100g.HasValue &&
                               nutriments.SaturatedFat100g.HasValue
        };
    }

    public async Task<OffProduct?> GetFullProductByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{BaseUrl}/product/{barcode}.json";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("OFF returned {StatusCode} for full product fetch: {Barcode}", response.StatusCode, barcode);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var offResponse = JsonSerializer.Deserialize<OffFullResponse>(json, JsonOptions);

            if (offResponse?.Status != 1 || offResponse.Product == null)
            {
                _logger.LogInformation("Full product not found in OFF: {Barcode}", barcode);
                return null;
            }

            return MapToOffEntities(offResponse.Product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching full product from OFF: {Barcode}", barcode);
            return null;
        }
    }

    public async Task<List<OffProduct>> FetchFullIsraeliProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{BaseUrl}/search?countries_tags=israel&page={page}&page_size={pageSize}&json=1";
            _logger.LogInformation("Fetching full Israeli products page {Page} from OFF", page);

            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("OFF returned {StatusCode} for full Israeli products fetch", response.StatusCode);
                return new List<OffProduct>();
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var searchResponse = JsonSerializer.Deserialize<OffFullSearchResponse>(json, JsonOptions);

            if (searchResponse?.Products == null)
            {
                return new List<OffProduct>();
            }

            var products = searchResponse.Products
                .Where(p => !string.IsNullOrEmpty(p.Code))
                .Select(MapToOffEntities)
                .ToList();

            _logger.LogInformation("Fetched {Count} full Israeli products from page {Page}", products.Count, page);
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching full Israeli products from OFF page {Page}", page);
            return new List<OffProduct>();
        }
    }

    private OffProduct MapToOffEntities(OffFullProduct src)
    {
        var barcode = src.Code ?? string.Empty;
        var now = DateTime.UtcNow;

        var product = new OffProduct
        {
            Barcode = barcode,
            ProductName = src.ProductName,
            ProductNameHe = src.ProductNameHe,
            ProductNameEn = src.ProductNameEn,
            GenericName = src.GenericName,
            GenericNameHe = src.GenericNameHe,
            GenericNameEn = src.GenericNameEn,
            Brands = src.Brands,
            BrandsTags = src.BrandsTags != null ? string.Join(",", src.BrandsTags) : null,
            Quantity = src.Quantity,
            ServingSize = src.ServingSize,
            ServingQuantity = src.ServingQuantity,
            Categories = src.Categories,
            CategoriesTags = src.CategoriesTags,
            CategoriesHierarchy = src.CategoriesHierarchy != null ? string.Join(",", src.CategoriesHierarchy) : null,
            Labels = src.Labels,
            LabelsTags = src.LabelsTags,
            Stores = src.Stores,
            Countries = src.Countries,
            CountriesTags = src.CountriesTags,
            ManufacturingPlaces = src.ManufacturingPlaces,
            Origins = src.Origins,
            Packaging = src.Packaging,
            PackagingTags = src.PackagingTags,
            Completeness = src.Completeness,
            LastModifiedT = src.LastModifiedT.HasValue ? DateTimeOffset.FromUnixTimeSeconds(src.LastModifiedT.Value).UtcDateTime : null,
            CreatedT = src.CreatedT.HasValue ? DateTimeOffset.FromUnixTimeSeconds(src.CreatedT.Value).UtcDateTime : null,
            Creator = src.Creator,
            Editor = src.LastEditor,
            EditorsCount = src.EditorsTags?.Count,
            States = src.States,
            StatesTags = src.StatesTags,
            ImportedAt = now,
            LastSyncedAt = now
        };

        product.Nutrition = MapNutrition(barcode, src.Nutriments, src.NutritionDataPer, src.NutritionGradeFr, now);
        product.Ingredients = MapIngredients(barcode, src, now);
        product.Scores = MapScores(barcode, src, now);
        product.Images = MapImages(barcode, src, now);
        product.Environment = MapEnvironment(barcode, src, now);

        return product;
    }

    private static OffNutrition MapNutrition(string barcode, OffFullNutriments? n, string? dataPer, string? gradeFr, DateTime now)
    {
        if (n == null)
        {
            return new OffNutrition { Barcode = barcode, LastSyncedAt = now };
        }

        return new OffNutrition
        {
            Barcode = barcode,
            EnergyKcal100g = n.EnergyKcal100g,
            EnergyKj100g = n.EnergyKj100g,
            EnergyKcalServing = n.EnergyKcalServing,
            EnergyKjServing = n.EnergyKjServing,
            Fat100g = n.Fat100g,
            FatServing = n.FatServing,
            SaturatedFat100g = n.SaturatedFat100g,
            SaturatedFatServing = n.SaturatedFatServing,
            MonounsaturatedFat100g = n.MonounsaturatedFat100g,
            PolyunsaturatedFat100g = n.PolyunsaturatedFat100g,
            TransFat100g = n.TransFat100g,
            Cholesterol100g = n.Cholesterol100g,
            Omega3Fat100g = n.Omega3Fat100g,
            Omega6Fat100g = n.Omega6Fat100g,
            Carbohydrates100g = n.Carbohydrates100g,
            CarbohydratesServing = n.CarbohydratesServing,
            Sugars100g = n.Sugars100g,
            SugarsServing = n.SugarsServing,
            Starch100g = n.Starch100g,
            Polyols100g = n.Polyols100g,
            Fiber100g = n.Fiber100g,
            FiberServing = n.FiberServing,
            Proteins100g = n.Proteins100g,
            ProteinsServing = n.ProteinsServing,
            Salt100g = n.Salt100g,
            SaltServing = n.SaltServing,
            Sodium100g = n.Sodium100g,
            SodiumServing = n.SodiumServing,
            VitaminA100g = n.VitaminA100g,
            VitaminB1100g = n.VitaminB1100g,
            VitaminB2100g = n.VitaminB2100g,
            VitaminB6100g = n.VitaminB6100g,
            VitaminB9100g = n.VitaminB9100g,
            VitaminB12100g = n.VitaminB12100g,
            VitaminC100g = n.VitaminC100g,
            VitaminD100g = n.VitaminD100g,
            VitaminE100g = n.VitaminE100g,
            VitaminK100g = n.VitaminK100g,
            VitaminPp100g = n.VitaminPp100g,
            Calcium100g = n.Calcium100g,
            Iron100g = n.Iron100g,
            Magnesium100g = n.Magnesium100g,
            Zinc100g = n.Zinc100g,
            Phosphorus100g = n.Phosphorus100g,
            Potassium100g = n.Potassium100g,
            Iodine100g = n.Iodine100g,
            Selenium100g = n.Selenium100g,
            Copper100g = n.Copper100g,
            Manganese100g = n.Manganese100g,
            Fluoride100g = n.Fluoride100g,
            Caffeine100g = n.Caffeine100g,
            Taurine100g = n.Taurine100g,
            Alcohol100g = n.Alcohol100g,
            NutritionDataPer = dataPer,
            NutritionGradeFr = gradeFr,
            LastSyncedAt = now
        };
    }

    private static OffIngredients MapIngredients(string barcode, OffFullProduct src, DateTime now)
    {
        return new OffIngredients
        {
            Barcode = barcode,
            IngredientsText = src.IngredientsText,
            IngredientsTextHe = src.IngredientsTextHe,
            IngredientsTextEn = src.IngredientsTextEn,
            IngredientsParsed = null,
            IngredientsCount = src.IngredientsN.HasValue ? (int)src.IngredientsN.Value : null,
            IngredientsPercentAnalysis = src.IngredientsPercentAnalysis,
            Allergens = src.Allergens?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
            AllergensTags = src.AllergensTags,
            AllergensHierarchy = src.AllergensHierarchy != null ? string.Join(",", src.AllergensHierarchy) : null,
            Traces = src.Traces?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
            TracesTags = src.TracesTags,
            Additives = null,
            AdditivesTags = src.AdditivesTags,
            AdditivesCount = src.AdditivesN.HasValue ? (int)src.AdditivesN.Value : null,
            AminoAcidsTags = src.AminoAcidsTags,
            MineralsTags = src.MineralsTags,
            VitaminsTags = src.VitaminsTags,
            NucleotidesTags = src.NucleotidesTags,
            OtherNutritionalSubstancesTags = src.OtherNutritionalSubstancesTags,
            NovaGroup = src.NovaGroup.HasValue ? (int)src.NovaGroup.Value : null,
            NovaGroupsMarkers = src.NovaGroupsMarkers?.ToString(),
            NovaGroupsTags = src.NovaGroupsTags,
            IsPalmOilFree = src.IngredientsAnalysisTags?.Contains("en:palm-oil-free"),
            IsVegan = src.IngredientsAnalysisTags?.Contains("en:vegan"),
            IsVegetarian = src.IngredientsAnalysisTags?.Contains("en:vegetarian"),
            VeganAnalysis = src.IngredientsAnalysisTags?.FirstOrDefault(t => t.Contains("vegan")),
            VegetarianAnalysis = src.IngredientsAnalysisTags?.FirstOrDefault(t => t.Contains("vegetarian")),
            IngredientsAnalysis = null,
            IngredientsAnalysisTags = src.IngredientsAnalysisTags,
            LastSyncedAt = now
        };
    }

    private static OffIngredientItem MapIngredientItem(OffFullIngredient src)
    {
        return new OffIngredientItem
        {
            Id = src.Id,
            Text = src.Text,
            Percent = src.Percent,
            PercentMin = src.PercentMin,
            PercentMax = src.PercentMax,
            PercentEstimate = src.PercentEstimate,
            Vegan = src.Vegan,
            Vegetarian = src.Vegetarian,
            FromPalmOil = src.FromPalmOil == "yes",
            Ingredients = src.Ingredients?.Select(MapIngredientItem).ToList()
        };
    }

    private static OffScores MapScores(string barcode, OffFullProduct src, DateTime now)
    {
        var nutrientLevels = src.NutrientLevels;

        string? grade2021 = null;
        int? score2021 = null;
        if (src.Nutriscore2021Tags?.Count > 0)
        {
            grade2021 = src.Nutriscore2021Tags.FirstOrDefault()?.Replace("nutriscore-", "").ToUpper();
        }

        string? grade2023 = null;
        int? score2023 = null;
        if (src.Nutriscore2023Tags?.Count > 0)
        {
            grade2023 = src.Nutriscore2023Tags.FirstOrDefault()?.Replace("nutriscore-", "").ToUpper();
        }

        return new OffScores
        {
            Barcode = barcode,
            NutriScoreGrade = src.NutriscoreGrade?.ToUpper(),
            NutriScoreScore = src.NutriscoreScore.HasValue ? (int)src.NutriscoreScore.Value : null,
            NutriScoreVersion = src.NutriscoreVersion,
            NutriscoreGrade2021 = grade2021,
            NutriscoreScore2021 = score2021,
            NutriscoreGrade2023 = grade2023,
            NutriscoreScore2023 = score2023,
            EcoScoreGrade = src.EcoscoreGrade?.ToUpper(),
            EcoScoreScore = src.EcoscoreScore.HasValue ? (int)src.EcoscoreScore.Value : null,
            EcoScoreVersion = null,
            EcoScoreAdjustments = src.EcoscoreData?.Adjustments != null ? 1 : null,
            EcoScorePackaging = src.EcoscoreData?.Adjustments?.Packaging?.Value.HasValue == true ? (int)src.EcoscoreData.Adjustments.Packaging.Value.Value : null,
            EcoScoreProduction = src.EcoscoreData?.Adjustments?.ProductionSystem?.Value.HasValue == true ? (int)src.EcoscoreData.Adjustments.ProductionSystem.Value.Value : null,
            EcoScoreOrigins = src.EcoscoreData?.Adjustments?.Origins?.EpiValue.HasValue == true ? (int)src.EcoscoreData.Adjustments.Origins.EpiValue.Value : null,
            EcoScoreThreatenedSpecies = src.EcoscoreData?.Adjustments?.ThreatenedSpecies?.Value.HasValue == true ? (int)src.EcoscoreData.Adjustments.ThreatenedSpecies.Value.Value : null,
            NovaGroup = src.NovaGroup.HasValue ? (int)src.NovaGroup.Value : null,
            NutrientLevelsEnergy = null,
            NutrientLevelsFat = nutrientLevels?.Fat,
            NutrientLevelsSaturatedFat = nutrientLevels?.SaturatedFat,
            NutrientLevelsSugars = nutrientLevels?.Sugars,
            NutrientLevelsSalt = nutrientLevels?.Salt,
            DataQualityErrorsCount = src.DataQualityErrorsTags?.Count,
            DataQualityErrorsTags = src.DataQualityErrorsTags,
            DataQualityWarningsCount = src.DataQualityWarningsTags?.Count,
            DataQualityWarningsTags = src.DataQualityWarningsTags,
            DataQualityInfoCount = src.DataQualityInfoTags?.Count,
            DataQualityInfoTags = src.DataQualityInfoTags,
            UnknownNutrientsCount = src.UnknownNutrientsTags?.Count,
            KnownNutrientsCount = null,
            LastSyncedAt = now
        };
    }

    private static OffImages MapImages(string barcode, OffFullProduct src, DateTime now)
    {
        return new OffImages
        {
            Barcode = barcode,
            ImageUrl = src.ImageUrl,
            ImageSmallUrl = src.ImageSmallUrl,
            ImageThumbUrl = src.ImageThumbUrl,
            ImageFrontUrl = src.ImageFrontUrl,
            ImageFrontSmallUrl = src.ImageFrontSmallUrl,
            ImageFrontThumbUrl = src.ImageFrontThumbUrl,
            ImageNutritionUrl = src.ImageNutritionUrl,
            ImageNutritionSmallUrl = src.ImageNutritionSmallUrl,
            ImageNutritionThumbUrl = src.ImageNutritionThumbUrl,
            ImageIngredientsUrl = src.ImageIngredientsUrl,
            ImageIngredientsSmallUrl = src.ImageIngredientsSmallUrl,
            ImageIngredientsThumbUrl = src.ImageIngredientsThumbUrl,
            ImagePackagingUrl = src.ImagePackagingUrl,
            ImagePackagingSmallUrl = src.ImagePackagingSmallUrl,
            ImagePackagingThumbUrl = src.ImagePackagingThumbUrl,
            SelectedImages = null,
            ImagesKeys = src.Images?.Keys.ToList(),
            ImagesCount = src.Images?.Count,
            LastSyncedAt = now
        };
    }

    private static OffEnvironment MapEnvironment(string barcode, OffFullProduct src, DateTime now)
    {
        var agribalyse = src.EcoscoreData?.Agribalyse;
        int? agribalyseFoodCode = null;
        if (int.TryParse(agribalyse?.AgribalyseFoodCode, out var code))
        {
            agribalyseFoodCode = code;
        }

        return new OffEnvironment
        {
            Barcode = barcode,
            CarbonFootprint100g = src.CarbonFootprintPercentOfKnownIngredients,
            CarbonFootprintServing = null,
            CarbonFootprintUnit = "g CO2e",
            CarbonFootprintSource = null,
            EnvironmentImpactLevel = src.EnvironmentImpactLevel != null ? decimal.TryParse(src.EnvironmentImpactLevel, out var level) ? level : null : null,
            EnvironmentImpactLevelTags = src.EnvironmentImpactLevelTags,
            PackagingRecycling = src.PackagingRecyclingTags != null ? string.Join(",", src.PackagingRecyclingTags) : null,
            PackagingComponents = null,
            PackagingMaterials = null,
            PackagingMaterialsTags = src.PackagingMaterialsTags,
            RecyclingInstruction = null,
            RecyclingInstructionToDiscard = null,
            RecyclingInstructionToRecycle = null,
            Origins = src.Origins,
            OriginsTags = null,
            ManufacturingPlaces = src.ManufacturingPlaces,
            ManufacturingPlacesTags = null,
            WaterFootprint100g = null,
            WaterFootprintUnit = null,
            AgribalyseFoodCode = agribalyseFoodCode,
            AgribalyseFoodName = agribalyse?.AgribalyseFoodNameEn,
            AgribalyseCo2Agriculture = agribalyse?.Co2Agriculture,
            AgribalyseCo2Consumption = agribalyse?.Co2Consumption,
            AgribalyseCo2Distribution = agribalyse?.Co2Distribution,
            AgribalyseCo2Packaging = agribalyse?.Co2Packaging,
            AgribalyseCo2Processing = agribalyse?.Co2Processing,
            AgribalyseCo2Transportation = agribalyse?.Co2Transportation,
            AgribalyseCo2Total = agribalyse?.Co2Total,
            AgribalyseEfSingleScore = agribalyse?.EfSingleScore,
            IsForestFootprintFree = null,
            ForestFootprint = null,
            LastSyncedAt = now
        };
    }
}

internal class OpenFoodFactsResponse
{
    public int Status { get; set; }
    public OpenFoodFactsProduct? Product { get; set; }
}

internal class OpenFoodFactsSearchResponse
{
    public List<OpenFoodFactsProduct>? Products { get; set; }
    public int Count { get; set; }
}

internal class OpenFoodFactsCountResponse
{
    public int Count { get; set; }
}

internal class OpenFoodFactsProduct
{
    public string? Code { get; set; }

    [JsonPropertyName("product_name")]
    public string? ProductName { get; set; }

    [JsonPropertyName("product_name_he")]
    public string? ProductNameHe { get; set; }

    [JsonPropertyName("product_name_en")]
    public string? ProductNameEn { get; set; }

    public string? Brands { get; set; }
    public string? Quantity { get; set; }
    public string? Categories { get; set; }

    [JsonPropertyName("ingredients_text")]
    public string? IngredientsText { get; set; }

    [JsonPropertyName("ingredients_text_he")]
    public string? IngredientsTextHe { get; set; }

    [JsonPropertyName("ingredients_text_en")]
    public string? IngredientsTextEn { get; set; }

    [JsonPropertyName("image_front_url")]
    public string? ImageFrontUrl { get; set; }

    [JsonPropertyName("image_nutrition_url")]
    public string? ImageNutritionUrl { get; set; }

    [JsonPropertyName("image_ingredients_url")]
    public string? ImageIngredientsUrl { get; set; }

    public OpenFoodFactsNutriments? Nutriments { get; set; }
}

internal class OpenFoodFactsNutriments
{
    [JsonPropertyName("energy-kcal_100g")]
    public decimal? EnergyKcal100g { get; set; }

    [JsonPropertyName("fat_100g")]
    public decimal? Fat100g { get; set; }

    [JsonPropertyName("saturated-fat_100g")]
    public decimal? SaturatedFat100g { get; set; }

    [JsonPropertyName("carbohydrates_100g")]
    public decimal? Carbohydrates100g { get; set; }

    [JsonPropertyName("sugars_100g")]
    public decimal? Sugars100g { get; set; }

    [JsonPropertyName("fiber_100g")]
    public decimal? Fiber100g { get; set; }

    [JsonPropertyName("proteins_100g")]
    public decimal? Proteins100g { get; set; }

    [JsonPropertyName("sodium_100g")]
    public decimal? Sodium100g { get; set; }
}
