using KMStore.Application.Abstractions;
using KMStore.Application.DTOs.Categories;
using KMStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KMStore.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly KMStoreDbContext _db;

    public CategoryRepository(KMStoreDbContext db)
    {
        _db = db;
    }

    public Task AddCategoryAsync(Category category)
    {
        _db.Categories.Add(category);
        return Task.CompletedTask;
    }

    public Task AddCategoryTranslationAsync(CategoryTranslation translation)
    {
        _db.CategoryTranslations.Add(translation);
        return Task.CompletedTask;
    }

    public async Task<bool> CategoryNameExistsAsync(string languageCode, string name, int? exceptCategoryId = null)
    {
        return await _db.CategoryTranslations.AnyAsync(x =>
            x.LanguageCode == languageCode &&
            x.Name == name &&
            (exceptCategoryId == null || x.CategoryId != exceptCategoryId.Value));
    }

    public async Task<Category?> GetCategoryWithTranslationsAsync(int categoryId)
    {
        return await _db.Categories
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == categoryId);
    }

    public async Task<List<CategoryListItem>> GetCategoriesByLangAsync(string languageCode)
    {
        return await _db.Categories
            .Select(c => new CategoryListItem
            {
                Id = c.Id,
                IsActive = c.IsActive,
                Name = c.Translations
                    .Where(t => t.LanguageCode == languageCode)
                    .Select(t => t.Name)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }


    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
