using HealthScan.Domain.ValueObjects;

namespace HealthScan.Domain.Interfaces;

public interface INutritionParser
{
    NutritionData ParseNutritionText(string text);
    double GetConfidence();
}
