using HealthScan.Domain.ValueObjects;

namespace HealthScan.Domain.Interfaces;

public interface IIngredientAnalyzer
{
    List<string> ParseIngredients(string ingredientsText);
    List<ScoreFlag> AnalyzeIngredients(string ingredientsText);
    List<ScoreFlag> AnalyzeIngredients(List<string> ingredients);
}
