using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NextBuy.Data;
using NextBuy.Models;

namespace NextBuy.Areas.Admin.Pages.Products;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Product> Product { get;set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int? CategoryId { get; set; }

    public async Task OnGetAsync()
    {
        var products = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrEmpty(SearchString))
        {
            products = products.Where(s => s.Name.Contains(SearchString));
        }
        
        if (CategoryId.HasValue)
        {
            products = products.Where(x => x.CategoryId == CategoryId);
        }

        Product = await products.ToListAsync();
        
        ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Categories, "Id", "Name");
    }
}
