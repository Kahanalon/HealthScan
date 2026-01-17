using HealthScan.Application.DTOs;
using HealthScan.Domain.Entities;
using HealthScan.Domain.Interfaces;

namespace HealthScan.Application.Services;

public interface IProductService
{
    Task<ProductResult> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
    Task<ProductSearchResponseDto> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ContributionResponseDto> ContributeAsync(string barcode, ContributionRequestDto request, CancellationToken cancellationToken = default);
}

public record ProductResult
{
    public bool Found { get; init; }
    public bool IsComplete { get; init; }
    public ProductDto? Product { get; init; }
    public ProductNotFoundDto? NotFound { get; init; }
    public ProductIncompleteDto? Incomplete { get; init; }
}

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductDataSource _dataSource;
    private readonly IScoringEngine _scoringEngine;
    private readonly IContributionRepository _contributionRepository;
    private readonly ICacheService _cacheService;

    public ProductService(
        IProductRepository productRepository,
        IProductDataSource dataSource,
        IScoringEngine scoringEngine,
        IContributionRepository contributionRepository,
        ICacheService cacheService)
    {
        _productRepository = productRepository;
        _dataSource = dataSource;
        _scoringEngine = scoringEngine;
        _contributionRepository = contributionRepository;
        _cacheService = cacheService;
    }

    public async Task<ProductResult> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"product:{barcode}";
        var cached = await _cacheService.GetAsync<ProductDto>(cacheKey, cancellationToken);
        if (cached != null)
        {
            return new ProductResult { Found = true, IsComplete = true, Product = cached };
        }

        var product = await _productRepository.GetByBarcodeAsync(barcode, cancellationToken);

        if (product == null)
        {
            product = await _dataSource.GetByBarcodeAsync(barcode, cancellationToken);
            if (product != null)
            {
                product = await _productRepository.AddAsync(product, cancellationToken);
            }
        }

        if (product == null)
        {
            return new ProductResult
            {
                Found = false,
                NotFound = new ProductNotFoundDto { Barcode = barcode }
            };
        }

        var scoreResult = _scoringEngine.CalculateScore(product);

        if (!scoreResult.IsComplete)
        {
            return new ProductResult
            {
                Found = true,
                IsComplete = false,
                Incomplete = new ProductIncompleteDto
                {
                    Barcode = product.Barcode,
                    Name = product.DisplayName,
                    Score = new IncompleteScoreDto
                    {
                        MissingFields = scoreResult.MissingFields
                    }
                }
            };
        }

        var productDto = MapToDto(product, scoreResult);
        await _cacheService.SetAsync(cacheKey, productDto, TimeSpan.FromHours(1), cancellationToken);

        return new ProductResult { Found = true, IsComplete = true, Product = productDto };
    }

    public async Task<ProductSearchResponseDto> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.SearchAsync(query, page, pageSize, cancellationToken);
        var totalCount = await _productRepository.GetSearchCountAsync(query, cancellationToken);

        var results = products.Select(p =>
        {
            var score = _scoringEngine.CalculateScore(p);
            return new ProductSearchResultDto
            {
                Barcode = p.Barcode,
                Name = p.DisplayName,
                Brand = p.Brand,
                Score = score.Value,
                Grade = score.Grade?.ToString()
            };
        }).ToList();

        return new ProductSearchResponseDto
        {
            Query = query,
            Results = results,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ContributionResponseDto> ContributeAsync(string barcode, ContributionRequestDto request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByBarcodeAsync(barcode, cancellationToken);

        var contribution = new ProductContribution
        {
            Id = Guid.NewGuid(),
            ProductId = product?.Id ?? Guid.Empty,
            Barcode = barcode,
            FieldName = request.FieldName,
            DeviceId = request.DeviceId,
            Status = "pending"
        };

        if (!string.IsNullOrEmpty(request.ImageBase64))
        {
            contribution.ImageUrl = $"pending:{contribution.Id}";
        }

        await _contributionRepository.AddAsync(contribution, cancellationToken);

        return new ContributionResponseDto
        {
            ContributionId = contribution.Id,
            Status = "pending"
        };
    }

    private static ProductDto MapToDto(Product product, Domain.ValueObjects.ScoreResult scoreResult)
    {
        return new ProductDto
        {
            Barcode = product.Barcode,
            Name = product.DisplayName,
            NameEn = product.NameEn,
            Brand = product.Brand,
            PackageSize = product.PackageSize,
            Score = new ScoreDto
            {
                Value = scoreResult.Value ?? 0,
                Grade = scoreResult.Grade?.ToString() ?? "?",
                Color = scoreResult.Color ?? "#808080",
                IsComplete = scoreResult.IsComplete
            },
            Flags = scoreResult.Flags.Select(f => new FlagDto
            {
                Type = f.Type.ToString(),
                Description = f.Description
            }).ToList(),
            TopReasons = scoreResult.TopReasons,
            Nutrition = new NutritionDto
            {
                Per100 = new NutritionPer100Dto
                {
                    Energy = product.Energy100g,
                    Fat = product.Fat100g,
                    SaturatedFat = product.SaturatedFat100g,
                    Carbohydrates = product.Carbohydrates100g,
                    Sugars = product.Sugars100g,
                    Fiber = product.Fiber100g,
                    Protein = product.Protein100g,
                    Sodium = product.Sodium100g
                },
                PerServing = product.ServingSize != null ? new NutritionPer100Dto
                {
                    Energy = product.EnergyServing,
                    Fat = product.FatServing,
                    SaturatedFat = product.SaturatedFatServing,
                    Carbohydrates = product.CarbohydratesServing,
                    Sugars = product.SugarsServing,
                    Fiber = product.FiberServing,
                    Protein = product.ProteinServing,
                    Sodium = product.SodiumServing
                } : null,
                ServingSize = product.ServingSize
            },
            LastUpdated = product.LastUpdated
        };
    }
}
