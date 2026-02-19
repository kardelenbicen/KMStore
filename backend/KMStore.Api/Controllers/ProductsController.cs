using KMStore.Application.DTOs.Products;
using KMStore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KMStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }

    // Admin: ürün ekle
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var result = await _service.CreateAsync(request);
        return Ok(result);
    }

    // Herkes: kategoriye göre ürün listele
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory([FromQuery] int categoryId, [FromQuery] string lang = "tr")
    {
        var result = await _service.GetByCategoryAsync(categoryId, lang);
        return Ok(result);
    }
    // Admin: ürün güncelle
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return Ok(result);
    }

    // Admin: ürün pasife çek (soft delete)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync(id);
        return NoContent();
    }

}
