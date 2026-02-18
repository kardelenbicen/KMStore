using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KMStore.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me() => Ok(new
    {
        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
        Email = User.FindFirstValue(ClaimTypes.Email),
        Role = User.FindFirstValue(ClaimTypes.Role)
    });

    [HttpGet("admin-only")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminOnly() => Ok("Admin erişti ✅");
}
