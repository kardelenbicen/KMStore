namespace KMStore.Application.DTOs.Products;

public record ProductResponse(
    int Id,
    int CategoryId,
    decimal Price,
    int Stock,
    bool IsActive,
    string LanguageCode,
    string Name,
    string Description
);
