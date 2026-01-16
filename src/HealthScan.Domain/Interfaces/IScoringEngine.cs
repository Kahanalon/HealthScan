using HealthScan.Domain.Entities;
using HealthScan.Domain.ValueObjects;

namespace HealthScan.Domain.Interfaces;

public interface IScoringEngine
{
    ScoreResult CalculateScore(Product product);
    ScoreResult CalculateScore(Product product, string? ingredientsText);
    bool CanScore(Product product);
}
