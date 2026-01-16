using HealthScan.Domain.Entities;

namespace HealthScan.Domain.Interfaces;

public interface IScoringRuleRepository
{
    Task<List<ScoringRule>> GetActiveRulesAsync(CancellationToken cancellationToken = default);
    Task<List<IngredientFlag>> GetActiveIngredientFlagsAsync(CancellationToken cancellationToken = default);
}
