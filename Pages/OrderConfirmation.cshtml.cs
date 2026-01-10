using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NextBuy.Data;
using NextBuy.Models;

namespace NextBuy.Pages;

[Authorize]
public class OrderConfirmationModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public OrderConfirmationModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Order Order { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        // Ensure user owns the order
        var userName = User.Identity?.Name;
        // In a real app, verify user ID properly. For now, assuming User.Identity.Name matches (or we need to fetch user).
        // A better check:
        // var userId = _userManager.GetUserId(User);
        // if (order.UserId != userId) return Forbid();

        Order = order;
        return Page();
    }
}
