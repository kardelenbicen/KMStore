using KMStore.Application.Abstractions;
using KMStore.Application.DTOs.Products;
using KMStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KMStore.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly KMStoreDbContext _db;

    public ProductRepository(KMStoreDbContext db)
    {
        _db = db;
    }

    public async Task<bool> CategoryExistsAsync(int categoryId)
        => await _db.Categories.AnyAsync(x => x.Id == categoryId);
    public async Task<bool> ProductExistsAsync(int productId)
    => await _db.Products.AnyAsync(x => x.Id == productId);

    public async Task<Product?> GetProductWithTranslationsAsync(int productId)
    {
        return await _db.Products
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == productId);
    }


    public Task AddProductAsync(Product product)
    {
        _db.Products.Add(product);
        return Task.CompletedTask;
    }

    public Task AddProductTranslationAsync(ProductTranslation translation)
    {
        _db.ProductTranslations.Add(translation);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
    public async Task<(List<ProductListItem> Items, int TotalCount)> GetProductsByCategoryPagedAsync(
    int categoryId, string languageCode, int page, int pageSize)
    {
        var query = _db.Products
            .Where(p => p.CategoryId == categoryId && p.IsActive);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductListItem
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
                Name = p.Translations
                    .Where(t => t.LanguageCode == languageCode)
                    .Select(t => t.Name)
                    .FirstOrDefault(),
                Description = p.Translations
                    .Where(t => t.LanguageCode == languageCode)
                    .Select(t => t.Description)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return (items, totalCount);
    }


    public async Task<List<ProductListItem>> GetProductsByCategoryAsync(int categoryId, string languageCode)
    {
        return await _db.Products
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .Select(p => new ProductListItem
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
                Name = p.Translations
                    .Where(t => t.LanguageCode == languageCode)
                    .Select(t => t.Name)
                    .FirstOrDefault(),
                Description = p.Translations
                    .Where(t => t.LanguageCode == languageCode)
                    .Select(t => t.Description)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }
}
