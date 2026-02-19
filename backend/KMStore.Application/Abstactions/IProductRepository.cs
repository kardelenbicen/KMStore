using KMStore.Application.DTOs.Products;
using KMStore.Domain.Entities;

namespace KMStore.Application.Abstractions;

public interface IProductRepository
{
    Task<bool> CategoryExistsAsync(int categoryId);
    Task AddProductAsync(Product product);
    Task AddProductTranslationAsync(ProductTranslation translation);
    Task SaveChangesAsync();
    Task<List<ProductListItem>> GetProductsByCategoryAsync(int categoryId, string languageCode);
    Task<(List<ProductListItem> Items, int TotalCount)> GetProductsByCategoryPagedAsync(
    int categoryId, string languageCode, int page, int pageSize);

    Task<Product?> GetProductWithTranslationsAsync(int productId);
    Task<bool> ProductExistsAsync(int productId);

}

