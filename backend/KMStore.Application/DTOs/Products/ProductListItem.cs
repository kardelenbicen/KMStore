namespace KMStore.Application.DTOs.Products;

public class ProductListItem
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
