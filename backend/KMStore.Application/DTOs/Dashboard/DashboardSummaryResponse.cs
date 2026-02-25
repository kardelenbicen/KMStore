using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMStore.Application.DTOs.Dashboard;

public class DashboardSummaryResponse
{
    public int TotalCategories { get; set; }
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int OutOfStockProducts { get; set; }
}