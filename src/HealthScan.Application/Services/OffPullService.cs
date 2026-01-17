using HealthScan.Domain.Entities;
using HealthScan.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HealthScan.Application.Services;

public interface IOffPullService
{
    Task<Product?> PullToProductAsync(string barcode, CancellationToken cancellationToken = default);
    Task<int> PullAllUnlinkedAsync(CancellationToken cancellationToken = default);
}

public class OffPullService : IOffPullService
{
    private readonly IOffProductRepository _offRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<OffPullService> _logger;

    public OffPullService(
        IOffProductRepository offRepository,
        IProductRepository productRepository,
        ILogger<OffPullService> logger)
    {
        _offRepository = offRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Product?> PullToProductAsync(string barcode, CancellationToken cancellationToken = default)
    {
        var offProduct = await _offRepository.GetByBarcodeAsync(barcode, cancellationToken);

        if (offProduct == null)
        {
            _logger.LogWarning("OFF product not found for pull: {Barcode}", barcode);
            return null;
        }

        var existingProduct = await _productRepository.GetByBarcodeAsync(barcode, cancellationToken);

        if (existingProduct == null)
        {
            var newProduct = CreateProductFromOff(offProduct);
            newProduct.OffBarcode = barcode;
            newProduct.OffSyncedAt = DateTime.UtcNow;
            await _productRepository.AddAsync(newProduct, cancellationToken);
            _logger.LogInformation("Created new product from OFF data: {Barcode}", barcode);
            return newProduct;
        }

        UpdateProductFromOff(existingProduct, offProduct);
        existingProduct.OffBarcode = barcode;
        existingProduct.OffSyncedAt = DateTime.UtcNow;
        await _productRepository.UpdateAsync(existingProduct, cancellationToken);
        _logger.LogInformation("Updated existing product with OFF data: {Barcode}", barcode);
        return existingProduct;
    }

    public async Task<int> PullAllUnlinkedAsync(CancellationToken cancellationToken = default)
    {
        var offProducts = await _offRepository.GetAllAsync(0, 10000, cancellationToken);
        var pulledCount = 0;

        foreach (var offProduct in offProducts)
        {
            try
            {
                var existingProduct = await _productRepository.GetByBarcodeAsync(offProduct.Barcode, cancellationToken);

                if (existingProduct == null)
                {
                    var newProduct = CreateProductFromOff(offProduct);
                    newProduct.OffBarcode = offProduct.Barcode;
                    newProduct.OffSyncedAt = DateTime.UtcNow;
                    await _productRepository.AddAsync(newProduct, cancellationToken);
                    pulledCount++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to pull OFF product: {Barcode}", offProduct.Barcode);
            }
        }

        _logger.LogInformation("Pulled {Count} products from OFF data", pulledCount);
        return pulledCount;
    }

    private static Product CreateProductFromOff(Domain.Entities.OpenFoodFacts.OffProduct off)
    {
        var nutrition = off.Nutrition;
        var ingredients = off.Ingredients;
        var images = off.Images;

        return new Product
        {
            Id = Guid.NewGuid(),
            Barcode = off.Barcode,
            NameHe = off.ProductNameHe,
            NameEn = off.ProductNameEn ?? off.ProductName,
            Brand = off.Brands,
            PackageSize = off.Quantity,
            Category = off.Categories,
            Energy100g = nutrition?.EnergyKcal100g,
            Fat100g = nutrition?.Fat100g,
            SaturatedFat100g = nutrition?.SaturatedFat100g,
            Carbohydrates100g = nutrition?.Carbohydrates100g,
            Sugars100g = nutrition?.Sugars100g,
            Fiber100g = nutrition?.Fiber100g,
            Protein100g = nutrition?.Proteins100g,
            Sodium100g = nutrition?.Sodium100g.HasValue == true ? nutrition.Sodium100g * 1000 : null,
            ServingSize = off.ServingSize,
            EnergyServing = nutrition?.EnergyKcalServing,
            FatServing = nutrition?.FatServing,
            SaturatedFatServing = nutrition?.SaturatedFatServing,
            CarbohydratesServing = nutrition?.CarbohydratesServing,
            SugarsServing = nutrition?.SugarsServing,
            FiberServing = nutrition?.FiberServing,
            ProteinServing = nutrition?.ProteinsServing,
            SodiumServing = nutrition?.SodiumServing.HasValue == true ? nutrition.SodiumServing * 1000 : null,
            IngredientsTextHe = ingredients?.IngredientsTextHe,
            IngredientsTextEn = ingredients?.IngredientsTextEn ?? ingredients?.IngredientsText,
            Allergens = ingredients?.AllergensTags,
            ImageFrontUrl = images?.ImageFrontUrl,
            ImageNutritionUrl = images?.ImageNutritionUrl,
            ImageIngredientsUrl = images?.ImageIngredientsUrl,
            Source = "openfoodfacts",
            Status = Domain.Enums.ProductStatus.Pending,
            NutritionComplete = nutrition?.Sugars100g.HasValue == true &&
                               nutrition?.Sodium100g.HasValue == true &&
                               nutrition?.SaturatedFat100g.HasValue == true,
            LastUpdated = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static void UpdateProductFromOff(Product product, Domain.Entities.OpenFoodFacts.OffProduct off)
    {
        var nutrition = off.Nutrition;
        var ingredients = off.Ingredients;
        var images = off.Images;

        product.NameHe ??= off.ProductNameHe;
        product.NameEn ??= off.ProductNameEn ?? off.ProductName;
        product.Brand ??= off.Brands;
        product.PackageSize ??= off.Quantity;
        product.Category ??= off.Categories;

        product.Energy100g ??= nutrition?.EnergyKcal100g;
        product.Fat100g ??= nutrition?.Fat100g;
        product.SaturatedFat100g ??= nutrition?.SaturatedFat100g;
        product.Carbohydrates100g ??= nutrition?.Carbohydrates100g;
        product.Sugars100g ??= nutrition?.Sugars100g;
        product.Fiber100g ??= nutrition?.Fiber100g;
        product.Protein100g ??= nutrition?.Proteins100g;
        product.Sodium100g ??= nutrition?.Sodium100g.HasValue == true ? nutrition.Sodium100g * 1000 : null;

        product.ServingSize ??= off.ServingSize;
        product.EnergyServing ??= nutrition?.EnergyKcalServing;
        product.FatServing ??= nutrition?.FatServing;
        product.SaturatedFatServing ??= nutrition?.SaturatedFatServing;
        product.CarbohydratesServing ??= nutrition?.CarbohydratesServing;
        product.SugarsServing ??= nutrition?.SugarsServing;
        product.FiberServing ??= nutrition?.FiberServing;
        product.ProteinServing ??= nutrition?.ProteinsServing;
        product.SodiumServing ??= nutrition?.SodiumServing.HasValue == true ? nutrition.SodiumServing * 1000 : null;

        product.IngredientsTextHe ??= ingredients?.IngredientsTextHe;
        product.IngredientsTextEn ??= ingredients?.IngredientsTextEn ?? ingredients?.IngredientsText;
        product.Allergens ??= ingredients?.AllergensTags;

        product.ImageFrontUrl ??= images?.ImageFrontUrl;
        product.ImageNutritionUrl ??= images?.ImageNutritionUrl;
        product.ImageIngredientsUrl ??= images?.ImageIngredientsUrl;

        if (!product.NutritionComplete)
        {
            product.NutritionComplete = nutrition?.Sugars100g.HasValue == true &&
                                       nutrition?.Sodium100g.HasValue == true &&
                                       nutrition?.SaturatedFat100g.HasValue == true;
        }

        product.LastUpdated = DateTime.UtcNow;
    }
}
