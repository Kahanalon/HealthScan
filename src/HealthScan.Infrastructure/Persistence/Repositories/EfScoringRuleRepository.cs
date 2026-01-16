using HealthScan.Domain.Entities;
using HealthScan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthScan.Infrastructure.Persistence.Repositories;

public class EfScoringRuleRepository : IScoringRuleRepository
{
    private readonly AppDbContext _context;

    public EfScoringRuleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ScoringRule>> GetActiveRulesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ScoringRules
            .Where(r => r.IsActive)
            .OrderBy(r => r.Priority)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<IngredientFlag>> GetActiveIngredientFlagsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.IngredientFlags
            .Where(f => f.IsActive)
            .ToListAsync(cancellationToken);
    }
}
