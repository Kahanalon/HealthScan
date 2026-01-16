using HealthScan.Domain.Entities;
using HealthScan.Domain.Enums;
using HealthScan.Domain.Interfaces;
using HealthScan.Domain.ValueObjects;

namespace HealthScan.Application.Scoring;

public class CustomScoringEngine : IScoringEngine
{
    private readonly IIngredientAnalyzer _ingredientAnalyzer;
    private const int BaseScore = 100;

    public CustomScoringEngine(IIngredientAnalyzer ingredientAnalyzer)
    {
        _ingredientAnalyzer = ingredientAnalyzer;
    }

    public bool CanScore(Product product)
    {
        var nutrition = product.GetNutritionPer100();
        return nutrition.HasRequiredFieldsForScoring();
    }

    public ScoreResult CalculateScore(Product product)
    {
        var ingredientsText = product.IngredientsTextHe ?? product.IngredientsTextEn;
        return CalculateScore(product, ingredientsText);
    }

    public ScoreResult CalculateScore(Product product, string? ingredientsText)
    {
        var nutrition = product.GetNutritionPer100();

        if (!nutrition.HasRequiredFieldsForScoring())
        {
            return ScoreResult.Incomplete(nutrition.GetMissingFields());
        }

        var score = BaseScore;
        var flags = new List<ScoreFlag>();
        var reasons = new List<string>();

        ApplyNutritionPenalties(nutrition, ref score, flags, reasons);

        if (!string.IsNullOrWhiteSpace(ingredientsText))
        {
            ApplyIngredientPenalties(ingredientsText, ref score, flags, reasons);
        }

        ApplyNutritionBonuses(nutrition, ref score, reasons);

        score = Math.Clamp(score, 0, 100);

        return ScoreResult.FromScore(score, flags, reasons);
    }

    private void ApplyNutritionPenalties(NutritionData nutrition, ref int score, List<ScoreFlag> flags, List<string> reasons)
    {
        if (nutrition.Sugars > 15)
        {
            score -= 20;
            flags.Add(new ScoreFlag
            {
                Type = FlagType.HighSugar,
                Description = $"High sugar content ({nutrition.Sugars}g/100g)",
                PenaltyPoints = -20
            });
            reasons.Add($"High sugar content ({nutrition.Sugars}g per 100g)");
        }

        if (nutrition.Sodium > 500)
        {
            score -= 20;
            flags.Add(new ScoreFlag
            {
                Type = FlagType.HighSodium,
                Description = $"High sodium content ({nutrition.Sodium}mg/100g)",
                PenaltyPoints = -20
            });
            reasons.Add($"High sodium content ({nutrition.Sodium}mg per 100g)");
        }

        if (nutrition.SaturatedFat > 5)
        {
            score -= 20;
            flags.Add(new ScoreFlag
            {
                Type = FlagType.HighSaturatedFat,
                Description = $"High saturated fat ({nutrition.SaturatedFat}g/100g)",
                PenaltyPoints = -20
            });
            reasons.Add($"High saturated fat ({nutrition.SaturatedFat}g per 100g)");
        }

        if (nutrition.Fiber < 1)
        {
            score -= 5;
            flags.Add(new ScoreFlag
            {
                Type = FlagType.LowFiber,
                Description = "Low fiber content",
                PenaltyPoints = -5
            });
        }
    }

    private void ApplyIngredientPenalties(string ingredientsText, ref int score, List<ScoreFlag> flags, List<string> reasons)
    {
        var ingredientFlags = _ingredientAnalyzer.AnalyzeIngredients(ingredientsText);

        foreach (var flag in ingredientFlags)
        {
            score += flag.PenaltyPoints;
            flags.Add(flag);
            reasons.Add(flag.Description);
        }
    }

    private void ApplyNutritionBonuses(NutritionData nutrition, ref int score, List<string> reasons)
    {
        if (nutrition.Fiber >= 5)
        {
            score += 10;
            reasons.Add($"Good source of fiber ({nutrition.Fiber}g per 100g)");
        }

        if (nutrition.Protein >= 10)
        {
            score += 10;
            reasons.Add($"Good source of protein ({nutrition.Protein}g per 100g)");
        }

        if (nutrition.Sugars < 5)
        {
            score += 5;
            reasons.Add("Low sugar content");
        }
    }
}
