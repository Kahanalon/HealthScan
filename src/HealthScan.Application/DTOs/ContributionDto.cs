namespace HealthScan.Application.DTOs;

public record ContributionRequestDto
{
    public string FieldName { get; init; } = string.Empty;
    public string? ImageBase64 { get; init; }
    public string? DeviceId { get; init; }
}

public record ContributionResponseDto
{
    public Guid ContributionId { get; init; }
    public string Status { get; init; } = "pending";
    public string Message { get; init; } = "Thank you! Your contribution is being processed.";
}
