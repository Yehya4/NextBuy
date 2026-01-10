using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NextBuy.Data;

namespace NextBuy.Areas.Admin.Pages;

[Authorize(Roles = "Admin")]
public class DashboardModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DashboardModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int ProductCount { get; set; }
    public int CategoryCount { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalRevenue { get; set; }

    public async Task OnGetAsync()
    {
        ProductCount = await _context.Products.CountAsync();
        CategoryCount = await _context.Categories.CountAsync();
        OrderCount = await _context.Orders.CountAsync();
        
        TotalRevenue = await _context.Orders
            .Where(o => o.Status != "Annulée")
            .SumAsync(o => o.TotalAmount);
    }
}
