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
    Task<Product?> GetProductWithTranslationsAsync(int productId);
    Task<bool> ProductExistsAsync(int productId);

}

