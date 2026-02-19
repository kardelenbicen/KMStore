using KMStore.Application.DTOs.Categories;
using KMStore.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KMStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService service)
    {
        _service = service;
    }

    // Admin: kategori ekle
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        var result = await _service.CreateAsync(request);
        return Ok(result);
    }

    // Herkes: kategori listele
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] string lang = "tr")
    {
        var result = await _service.GetAllAsync(lang);
        return Ok(result);
    }

    // Admin: kategori güncelle (dil bazlı translation günceller/ekler)
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return Ok(result);
    }
}
