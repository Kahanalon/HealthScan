namespace HealthScan.Application.DTOs;

public record ProductDto
{
    public string Barcode { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? NameEn { get; init; }
    public string? Brand { get; init; }
    public string? PackageSize { get; init; }
    public ScoreDto? Score { get; init; }
    public List<FlagDto> Flags { get; init; } = new();
    public List<string> TopReasons { get; init; } = new();
    public NutritionDto? Nutrition { get; init; }
    public string Disclaimer { get; init; } = "For informational purposes only. Not medical advice.";
    public DateTime? LastUpdated { get; init; }
}

public record ProductSearchResultDto
{
    public string Barcode { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? Brand { get; init; }
    public int? Score { get; init; }
    public string? Grade { get; init; }
}

public record ProductSearchResponseDto
{
    public string Query { get; init; } = string.Empty;
    public List<ProductSearchResultDto> Results { get; init; } = new();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}

public record ProductNotFoundDto
{
    public string Barcode { get; init; } = string.Empty;
    public string Status { get; init; } = "not_found";
    public string Message { get; init; } = "Product not found. Help us by contributing!";
}

public record ProductIncompleteDto
{
    public string Barcode { get; init; } = string.Empty;
    public string? Name { get; init; }
    public IncompleteScoreDto Score { get; init; } = new();
    public string Message { get; init; } = "Incomplete nutrition data. Please photograph the nutrition label.";
}

public record IncompleteScoreDto
{
    public int? Value { get; init; } = null;
    public string? Grade { get; init; } = null;
    public bool IsComplete { get; init; } = false;
    public List<string> MissingFields { get; init; } = new();
}
