namespace HealthScan.Domain.Entities;

public class ScoringRule
{
    public Guid Id { get; set; }
    public string RuleName { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public string ConditionJson { get; set; } = "{}";
    public int Points { get; set; }
    public string? DescriptionHe { get; set; }
    public string? DescriptionEn { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; } = true;

    public string GetDescription(string language = "en") =>
        language == "he" ? DescriptionHe ?? DescriptionEn ?? RuleName : DescriptionEn ?? DescriptionHe ?? RuleName;
}
