using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NextBuy.Data;
using NextBuy.Models;
using NextBuy.Extensions;

namespace NextBuy.Pages;

public class ProductDetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ProductDetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Product Product { get; set; } = default!;
    public List<Product> RelatedProducts { get; set; } = new List<Product>();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null)
        {
            return NotFound();
        }
        Product = product;

        // Fetch related products
        if (Product.CategoryId != 0)
        {
            RelatedProducts = await _context.Products
                .Where(p => p.CategoryId == Product.CategoryId && p.Id != Product.Id)
                .Take(4)
                .ToListAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
            cart.Add(id);
            HttpContext.Session.Set("Cart", cart);
        }
        return RedirectToPage("./Cart");
    }
}
