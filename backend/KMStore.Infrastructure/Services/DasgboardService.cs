using KMStore.Application.DTOs.Dashboard;
using KMStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KMStore.Infrastructure.Services;

public class DashboardService
{
    private readonly KMStoreDbContext _db;

    public DashboardService(KMStoreDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummaryResponse> GetSummaryAsync()
    {
        var totalCategories = await _db.Categories.CountAsync();
        var totalProducts = await _db.Products.CountAsync();
        var activeProducts = await _db.Products.CountAsync(p => p.IsActive);
        var outOfStockProducts = await _db.Products.CountAsync(p => p.IsActive && p.Stock <= 0);

        return new DashboardSummaryResponse
        {
            TotalCategories = totalCategories,
            TotalProducts = totalProducts,
            ActiveProducts = activeProducts,
            OutOfStockProducts = outOfStockProducts
        };
    }
}