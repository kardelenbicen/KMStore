namespace KMStore.Application.DTOs.Products;

public record CreateProductRequest(
    int CategoryId,
    decimal Price,
    int Stock,
    string LanguageCode,
    string Name,
    string Description
);
