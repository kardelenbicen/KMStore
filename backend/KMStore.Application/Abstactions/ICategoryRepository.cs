using KMStore.Application.DTOs.Categories;
using KMStore.Domain.Entities;


namespace KMStore.Application.Abstractions;

public interface ICategoryRepository
{
    Task<bool> CategoryNameExistsAsync(string languageCode, string name, int? exceptCategoryId = null);
    Task AddCategoryAsync(Category category);
    Task AddCategoryTranslationAsync(CategoryTranslation translation);
    Task<Category?> GetCategoryWithTranslationsAsync(int categoryId);
    Task<List<CategoryListItem>> GetCategoriesByLangAsync(string languageCode);
    Task SaveChangesAsync();
}
