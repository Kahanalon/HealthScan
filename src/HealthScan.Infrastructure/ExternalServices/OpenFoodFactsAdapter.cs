using System.Text.Json;
using System.Text.Json.Serialization;
using HealthScan.Domain.Entities;
using HealthScan.Domain.Enums;
using HealthScan.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HealthScan.Infrastructure.ExternalServices;

public class OpenFoodFactsAdapter : IProductDataSource
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenFoodFactsAdapter> _logger;
    private const string BaseUrl = "https://world.openfoodfacts.org/api/v2";

    public string SourceName => "openfoodfacts";

    public OpenFoodFactsAdapter(HttpClient httpClient, ILogger<OpenFoodFactsAdapter> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "HealthScan/1.0 (contact@healthscan.app)");
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
}

internal class OpenFoodFactsResponse
{
    public int Status { get; set; }
    public OpenFoodFactsProduct? Product { get; set; }
}

internal class OpenFoodFactsSearchResponse
{
    public List<OpenFoodFactsProduct>? Products { get; set; }
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
