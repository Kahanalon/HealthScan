using HealthScan.Domain.Entities;
using HealthScan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthScan.Infrastructure.Persistence.Repositories;

public class EfContributionRepository : IContributionRepository
{
    private readonly AppDbContext _context;

    public EfContributionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductContribution> AddAsync(ProductContribution contribution, CancellationToken cancellationToken = default)
    {
        contribution.Id = Guid.NewGuid();
        contribution.CreatedAt = DateTime.UtcNow;

        _context.ProductContributions.Add(contribution);
        await _context.SaveChangesAsync(cancellationToken);

        return contribution;
    }

    public async Task<List<ProductContribution>> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return await _context.ProductContributions
            .Where(c => c.Barcode == barcode)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ProductContribution>> GetPendingAsync(int limit = 100, CancellationToken cancellationToken = default)
    {
        return await _context.ProductContributions
            .Where(c => c.Status == "pending")
            .OrderBy(c => c.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductContribution> UpdateStatusAsync(Guid id, string status, CancellationToken cancellationToken = default)
    {
        var contribution = await _context.ProductContributions
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new InvalidOperationException($"Contribution {id} not found");

        contribution.Status = status;
        await _context.SaveChangesAsync(cancellationToken);

        return contribution;
    }
}
