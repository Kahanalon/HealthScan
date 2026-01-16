using System.Text.RegularExpressions;
using HealthScan.Domain.Enums;
using HealthScan.Domain.Interfaces;
using HealthScan.Domain.ValueObjects;

namespace HealthScan.Application.Scoring;

public class RegexIngredientAnalyzer : IIngredientAnalyzer
{
    private static readonly Dictionary<FlagType, (string[] Patterns, int Penalty, string Description)> FlagDefinitions = new()
    {
        [FlagType.ArtificialSweetener] = (
            new[] { "aspartame", "אספרטיים", "sucralose", "סוכרלוז", "acesulfame", "אצסולפם", "saccharin", "סכרין", "stevia", "סטביה" },
            -10,
            "Contains artificial sweeteners"
        ),
        [FlagType.ArtificialColorant] = (
            new[] { @"E1[0-4]\d", "tartrazine", "טרטרזין", "sunset yellow", "צהוב שקיעה", "brilliant blue", "allura red" },
            -10,
            "Contains artificial colorants"
        ),
        [FlagType.FlavorEnhancer] = (
            new[] { @"E6[0-2]\d", "MSG", "monosodium glutamate", "גלוטמט", "disodium guanylate", "disodium inosinate" },
            -8,
            "Contains flavor enhancers (MSG)"
        ),
        [FlagType.PalmOil] = (
            new[] { "palm oil", "שמן דקלים", "palm fat", "שומן דקלים", "palmitate" },
            -5,
            "Contains palm oil"
        ),
        [FlagType.Preservative] = (
            new[] { @"E2\d{2}", "sodium benzoate", "בנזואט", "potassium sorbate", "סורבט", "sodium nitrite", "sodium nitrate" },
            -5,
            "Contains preservatives"
        ),
        [FlagType.Emulsifier] = (
            new[] { @"E4\d{2}", "lecithin", "לציטין", "mono and diglycerides", "polysorbate" },
            -3,
            "Contains emulsifiers"
        ),
        [FlagType.TransFat] = (
            new[] { "trans fat", "partially hydrogenated", "שומן טראנס", "מוקשה חלקית" },
            -15,
            "Contains trans fats"
        )
    };

    public List<string> ParseIngredients(string ingredientsText)
    {
        if (string.IsNullOrWhiteSpace(ingredientsText))
            return new List<string>();

        var delimiters = new[] { ',', ';', '•', '،', '\n' };
        var ingredients = ingredientsText
            .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
            .Select(i => CleanIngredient(i))
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .ToList();

        return ingredients;
    }

    public List<ScoreFlag> AnalyzeIngredients(string ingredientsText)
    {
        if (string.IsNullOrWhiteSpace(ingredientsText))
            return new List<ScoreFlag>();

        var normalizedText = ingredientsText.ToLowerInvariant();
        var flags = new List<ScoreFlag>();
        var detectedTypes = new HashSet<FlagType>();

        foreach (var (flagType, (patterns, penalty, description)) in FlagDefinitions)
        {
            if (detectedTypes.Contains(flagType))
                continue;

            foreach (var pattern in patterns)
            {
                bool isMatch;
                if (pattern.StartsWith("E") && pattern.Contains("\\d"))
                {
                    isMatch = Regex.IsMatch(normalizedText, pattern, RegexOptions.IgnoreCase);
                }
                else
                {
                    isMatch = normalizedText.Contains(pattern.ToLowerInvariant());
                }

                if (isMatch)
                {
                    flags.Add(new ScoreFlag
                    {
                        Type = flagType,
                        Description = description,
                        PenaltyPoints = penalty
                    });
                    detectedTypes.Add(flagType);
                    break;
                }
            }
        }

        return flags;
    }

    public List<ScoreFlag> AnalyzeIngredients(List<string> ingredients)
    {
        var combined = string.Join(", ", ingredients);
        return AnalyzeIngredients(combined);
    }

    private static string CleanIngredient(string ingredient)
    {
        var cleaned = Regex.Replace(ingredient, @"\d+%?", "").Trim();
        cleaned = Regex.Replace(cleaned, @"[\(\)\[\]]", "").Trim();
        return cleaned;
    }
}
