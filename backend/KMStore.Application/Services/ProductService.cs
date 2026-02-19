using KMStore.Application.Abstractions;
using KMStore.Application.DTOs.Products;
using KMStore.Application.DTOs.Common;
using KMStore.Domain.Entities;
using KMStore.Application.Exceptions;

namespace KMStore.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        var lang = request.LanguageCode.Trim().ToLower();
        var name = request.Name.Trim();
        var desc = request.Description.Trim();

        if (request.Price <= 0) throw new BadRequestException("Price must be greater than 0.");
        if (request.Stock < 0) throw new BadRequestException("Stock cannot be negative.");
        if (string.IsNullOrWhiteSpace(lang)) throw new BadRequestException("LanguageCode is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new BadRequestException("Name is required.");
        if (string.IsNullOrWhiteSpace(desc)) throw new BadRequestException("Description is required.");

        var categoryExists = await _repo.CategoryExistsAsync(request.CategoryId);
        if (!categoryExists) throw new NotFoundException("Category not found.");



        var product = new Product
        {
            CategoryId = request.CategoryId,
            Price = request.Price,
            Stock = request.Stock,
            IsActive = true
            // CreatedAt/CreatAt sende hangisiyse otomatik set oluyor
        };

        await _repo.AddProductAsync(product);

        var translation = new ProductTranslation
        {
            Product = product,
            LanguageCode = lang,
            Name = name,
            Description = desc
        };
        await _repo.AddProductTranslationAsync(translation);

        await _repo.SaveChangesAsync();

        return new ProductResponse(
            product.Id,
            product.CategoryId,
            product.Price,
            product.Stock,
            product.IsActive,
            lang,
            name,
            desc
        );
    }
    public async Task<ProductResponse> UpdateAsync(int id, UpdateProductRequest request)
    {
        var lang = request.LanguageCode.Trim().ToLower();
        var name = request.Name.Trim();
        var desc = request.Description.Trim();

        if (request.Price <= 0) throw new Exception("Price must be greater than 0.");
        if (request.Stock < 0) throw new Exception("Stock cannot be negative.");
        if (string.IsNullOrWhiteSpace(lang)) throw new Exception("LanguageCode is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name is required.");
        if (string.IsNullOrWhiteSpace(desc)) throw new Exception("Description is required.");

        var product = await _repo.GetProductWithTranslationsAsync(id);
        if (product is null) throw new NotFoundException("Product not found.");

        product.Price = request.Price;
        product.Stock = request.Stock;

        if (request.IsActive.HasValue)
            product.IsActive = request.IsActive.Value;

        var tr = product.Translations.FirstOrDefault(t => t.LanguageCode == lang);
        if (tr is null)
        {
            tr = new ProductTranslation
            {
                ProductId = id,
                LanguageCode = lang,
                Name = name,
                Description = desc
            };
            await _repo.AddProductTranslationAsync(tr);
        }
        else
        {
            tr.Name = name;
            tr.Description = desc;
        }

        await _repo.SaveChangesAsync();

        return new ProductResponse(product.Id, product.CategoryId, product.Price, product.Stock, product.IsActive, lang, name, desc);
    }

    public async Task SoftDeleteAsync(int id)
    {
        var product = await _repo.GetProductWithTranslationsAsync(id);
        if (product is null) throw new NotFoundException("Product not found.");

        product.IsActive = false;
        await _repo.SaveChangesAsync();
    }
    public async Task<PagedResponse<ProductResponse>> GetByCategoryPagedAsync(int categoryId, string lang, int page, int pageSize)
    {
        lang = (lang ?? "tr").Trim().ToLower();

        if (categoryId <= 0) throw new BadRequestException("categoryId is required.");
        if (page <= 0) throw new BadRequestException("page must be >= 1.");
        if (pageSize <= 0) throw new BadRequestException("pageSize must be >= 1.");
        if (pageSize > 50) throw new BadRequestException("pageSize must be <= 50.");

        var categoryExists = await _repo.CategoryExistsAsync(categoryId);
        if (!categoryExists) throw new NotFoundException("Category not found.");

        var (items, totalCount) = await _repo.GetProductsByCategoryPagedAsync(categoryId, lang, page, pageSize);

        var mapped = items
            .Where(x => x.Name != null)
            .Select(x => new ProductResponse(
                x.Id,
                x.CategoryId,
                x.Price,
                x.Stock,
                x.IsActive,
                lang,
                x.Name!,
                x.Description ?? ""
            ))
            .ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResponse<ProductResponse>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Items = mapped
        };
    }



    public async Task<List<ProductResponse>> GetByCategoryAsync(int categoryId, string lang)
    {
        lang = (lang ?? "tr").Trim().ToLower();
        if (categoryId <= 0) throw new Exception("categoryId is required.");

        var categoryExists = await _repo.CategoryExistsAsync(categoryId);
        if (!categoryExists) throw new NotFoundException("Category not found.");


        var list = await _repo.GetProductsByCategoryAsync(categoryId, lang);

        return list
            .Where(x => x.Name != null)
            .Select(x => new ProductResponse(
                x.Id,
                x.CategoryId,
                x.Price,
                x.Stock,
                x.IsActive,
                lang,
                x.Name!,
                x.Description ?? ""
            ))
            .ToList();
    }


}
