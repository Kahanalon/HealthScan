namespace HealthScan.Application.DTOs;

public record ScoreDto
{
    public int Value { get; init; }
    public string Grade { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public bool IsComplete { get; init; } = true;
}

public record FlagDto
{
    public string Type { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
