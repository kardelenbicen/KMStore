namespace KMStore.Application.DTOs.Products;

public record UpdateProductRequest(
    decimal Price,
    int Stock,
    string LanguageCode,
    string Name,
    string Description,
    bool? IsActive // opsiyonel: istersen buradan da pasife çekebil
);
