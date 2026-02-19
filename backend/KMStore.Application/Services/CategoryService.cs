using KMStore.Application.Abstractions;
using KMStore.Application.DTOs.Categories;
using KMStore.Domain.Entities;

namespace KMStore.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
    {
        var lang = request.LanguageCode.Trim().ToLower();
        var name = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(lang)) throw new Exception("LanguageCode is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name is required.");

        var exists = await _repo.CategoryNameExistsAsync(lang, name);
        if (exists) throw new Exception("Category name already exists for this language.");

        var category = new Category();
        await _repo.AddCategoryAsync(category);

        var translation = new CategoryTranslation
        {
            Category = category,
            LanguageCode = lang,
            Name = name
        };
        await _repo.AddCategoryTranslationAsync(translation);

        await _repo.SaveChangesAsync();

        return new CategoryResponse(category.Id, name, lang, category.IsActive);
    }

    public async Task<List<CategoryResponse>> GetAllAsync(string lang)
    {
        lang = (lang ?? "tr").Trim().ToLower();

        var list = await _repo.GetCategoriesByLangAsync(lang);

        return list
            .Where(x => x.Name != null)
            .Select(x => new CategoryResponse(x.Id, x.Name!, lang, x.IsActive))
            .ToList();
    }

    public async Task<CategoryResponse> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        var lang = request.LanguageCode.Trim().ToLower();
        var name = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(lang)) throw new Exception("LanguageCode is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name is required.");

        var category = await _repo.GetCategoryWithTranslationsAsync(id);
        if (category is null) throw new Exception("Category not found.");

        var exists = await _repo.CategoryNameExistsAsync(lang, name, exceptCategoryId: id);
        if (exists) throw new Exception("Category name already exists for this language.");

        var tr = category.Translations.FirstOrDefault(t => t.LanguageCode == lang);

        if (tr is null)
        {
            tr = new CategoryTranslation
            {
                CategoryId = id,
                LanguageCode = lang,
                Name = name
            };
            await _repo.AddCategoryTranslationAsync(tr);
        }
        else
        {
            tr.Name = name;
        }

        await _repo.SaveChangesAsync();
        return new CategoryResponse(category.Id, name, lang, category.IsActive);
    }
}
