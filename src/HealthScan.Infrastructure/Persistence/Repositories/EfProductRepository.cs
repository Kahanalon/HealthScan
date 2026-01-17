using HealthScan.Domain.Entities;
using HealthScan.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthScan.Infrastructure.Persistence.Repositories;

public class EfProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public EfProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Barcode == barcode, cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<Product>> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var skip = (page - 1) * pageSize;
        var normalizedQuery = query.ToLower();

        return await _context.Products
            .Where(p =>
                (p.NameHe != null && p.NameHe.ToLower().Contains(normalizedQuery)) ||
                (p.NameEn != null && p.NameEn.ToLower().Contains(normalizedQuery)) ||
                (p.Brand != null && p.Brand.ToLower().Contains(normalizedQuery)) ||
                p.Barcode.Contains(query))
            .OrderByDescending(p => p.LastUpdated)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetSearchCountAsync(string query, CancellationToken cancellationToken = default)
    {
        var normalizedQuery = query.ToLower();

        return await _context.Products
            .CountAsync(p =>
                (p.NameHe != null && p.NameHe.ToLower().Contains(normalizedQuery)) ||
                (p.NameEn != null && p.NameEn.ToLower().Contains(normalizedQuery)) ||
                (p.Brand != null && p.Brand.ToLower().Contains(normalizedQuery)) ||
                p.Barcode.Contains(query), cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;
        product.LastUpdated = DateTime.UtcNow;

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.LastUpdated = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task<bool> ExistsAsync(string barcode, CancellationToken cancellationToken = default)
    {
        return await _context.Products.AnyAsync(p => p.Barcode == barcode, cancellationToken);
    }

    public async Task<int> BulkInsertAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default)
    {
        var existingBarcodes = await _context.Products
            .Select(p => p.Barcode)
            .ToHashSetAsync(cancellationToken);

        var newProducts = products
            .Where(p => !existingBarcodes.Contains(p.Barcode))
            .Select(p =>
            {
                p.Id = Guid.NewGuid();
                p.CreatedAt = DateTime.UtcNow;
                p.LastUpdated = DateTime.UtcNow;
                return p;
            })
            .ToList();

        if (newProducts.Count == 0)
            return 0;

        await _context.Products.AddRangeAsync(newProducts, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newProducts.Count;
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.CountAsync(cancellationToken);
    }
}
