using System.Text.RegularExpressions;
using HealthScan.Domain.Interfaces;
using HealthScan.Domain.ValueObjects;

namespace HealthScan.Application.Scoring;

public class NutritionTextParser : INutritionParser
{
    private double _lastConfidence;

    private static readonly Dictionary<string, Regex> Patterns = new()
    {
        ["energy"] = new Regex(@"(?:אנרגיה|energy|קלוריות|calories?|kcal)[:\s]*(\d+(?:[.,]\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
        ["fat"] = new Regex(@"(?:שומן(?:\s+כולל)?|total\s*fat|fat(?:s)?)[:\s]*(\d+(?:[.,]\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
        ["saturatedFat"] = new Regex(@"(?:שומן\s*רווי|saturated\s*fat)[:\s]*(\d+(?:[.,]\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
        ["carbs"] = new Regex(@"(?:פחמימות|carbohydrate?s?|total\s*carb)[:\s]*(\d+(?:[.,]\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
        ["sugars"] = new Regex(@"(?:סוכר(?:ים)?|sugar?s?)[:\s]*(\d+(?:[.,]\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
        ["protein"] = new Regex(@"(?:חלבון|protein)[:\s]*(\d+(?:[.,]\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
        ["sodium"] = new Regex(@"(?:נתרן|sodium)[:\s]*(\d+(?:[.,]\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
        ["fiber"] = new Regex(@"(?:סיבים\s*תזונתיים|סיבים|dietary\s*fiber|fiber)[:\s]*(\d+(?:[.,]\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Compiled)
    };

    public NutritionData ParseNutritionText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            _lastConfidence = 0;
            return new NutritionData();
        }

        var matchCount = 0;
        var totalFields = Patterns.Count;

        decimal? energy = ExtractValue(text, "energy", ref matchCount);
        decimal? fat = ExtractValue(text, "fat", ref matchCount);
        decimal? saturatedFat = ExtractValue(text, "saturatedFat", ref matchCount);
        decimal? carbs = ExtractValue(text, "carbs", ref matchCount);
        decimal? sugars = ExtractValue(text, "sugars", ref matchCount);
        decimal? protein = ExtractValue(text, "protein", ref matchCount);
        decimal? sodium = ExtractValue(text, "sodium", ref matchCount);
        decimal? fiber = ExtractValue(text, "fiber", ref matchCount);

        _lastConfidence = (double)matchCount / totalFields;

        return new NutritionData
        {
            Energy = energy,
            Fat = fat,
            SaturatedFat = saturatedFat,
            Carbohydrates = carbs,
            Sugars = sugars,
            Protein = protein,
            Sodium = sodium,
            Fiber = fiber
        };
    }

    public double GetConfidence() => _lastConfidence;

    private static decimal? ExtractValue(string text, string field, ref int matchCount)
    {
        if (!Patterns.TryGetValue(field, out var pattern))
            return null;

        var match = pattern.Match(text);
        if (!match.Success)
            return null;

        matchCount++;
        var valueStr = match.Groups[1].Value.Replace(',', '.');
        return decimal.TryParse(valueStr, out var value) ? value : null;
    }
}
