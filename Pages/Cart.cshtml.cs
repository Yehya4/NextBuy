using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NextBuy.Data;
using NextBuy.Models;
using NextBuy.Extensions;

namespace NextBuy.Pages;

public class CartModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CartModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<CartItemViewModel> CartItems { get; set; } = new();
    public decimal TotalAmount { get; set; }

    public async Task OnGetAsync()
    {
        var cartIds = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        
        if (cartIds.Any())
        {
            var products = await _context.Products.Where(p => cartIds.Contains(p.Id)).ToListAsync();
            
            // Group by Id to get quantity
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
        }
    }

    public IActionResult OnPostIncrease(int id)
    {
        var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        cart.Add(id);
        HttpContext.Session.Set("Cart", cart);
        return RedirectToPage();
    }

    public IActionResult OnPostDecrease(int id)
    {
        var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        cart.Remove(id);
        HttpContext.Session.Set("Cart", cart);
        return RedirectToPage();
    }

    public IActionResult OnPostRemove(int id)
    {
        var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        cart.RemoveAll(i => i == id);
        HttpContext.Session.Set("Cart", cart);
        return RedirectToPage();
    }
    
    public IActionResult OnPostClear()
    {
        HttpContext.Session.Remove("Cart");
        return RedirectToPage();
    }
}

public class CartItemViewModel
{
    public required Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Product.Price * Quantity;
}
