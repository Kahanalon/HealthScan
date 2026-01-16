using HealthScan.Domain.Enums;

namespace HealthScan.Domain.Entities;

public class IngredientFlag
{
    public Guid Id { get; set; }
    public string IngredientPattern { get; set; } = string.Empty;
    public FlagType FlagType { get; set; }
    public int PenaltyPoints { get; set; }
    public string? DescriptionHe { get; set; }
    public string? DescriptionEn { get; set; }
    public bool IsActive { get; set; } = true;

    public string GetDescription(string language = "en") =>
        language == "he" ? DescriptionHe ?? DescriptionEn ?? FlagType.ToString() : DescriptionEn ?? DescriptionHe ?? FlagType.ToString();
}
