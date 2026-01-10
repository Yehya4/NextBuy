using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NextBuy.Data;
using NextBuy.Models;
using NextBuy.Extensions;

namespace NextBuy.Pages;

[Authorize]
public class CheckoutModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckoutModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public List<CartItemViewModel> CartItems { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public ApplicationUser CurrentUser { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        CurrentUser = user;

        var cartIds = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        if (!cartIds.Any())
        {
            return RedirectToPage("./Cart");
        }

        var products = await _context.Products.Where(p => cartIds.Contains(p.Id)).ToListAsync();
        var groupedIds = cartIds.GroupBy(id => id);

        foreach (var group in groupedIds)
        {
            var product = products.FirstOrDefault(p => p.Id == group.Key);
            if (product != null)
            {
                CartItems.Add(new CartItemViewModel
                {
                    Product = product,
                    Quantity = group.Count()
                });
            }
        }
        TotalAmount = CartItems.Sum(i => i.Total);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var cartIds = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        if (!cartIds.Any())
        {
            return RedirectToPage("./Cart");
        }

        var products = await _context.Products.Where(p => cartIds.Contains(p.Id)).ToListAsync();
        var groupedIds = cartIds.GroupBy(id => id);
        
        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var group in groupedIds)
        {
            var product = products.FirstOrDefault(p => p.Id == group.Key);
            if (product != null)
            {
                var quantity = group.Count();
                var price = product.Price;
                totalAmount += price * quantity;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    Price = price
                });
            }
        }

        var order = new Order
        {
            UserId = user.Id,
            OrderDate = DateTime.Now,
            Status = "En attente",
            TotalAmount = totalAmount,
            OrderItems = orderItems
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Clear cart
        HttpContext.Session.Remove("Cart");

        return RedirectToPage("./OrderConfirmation", new { id = order.Id });
    }
}
