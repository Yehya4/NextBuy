using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NextBuy.Data;
using NextBuy.Models;
using NextBuy.Extensions;

namespace NextBuy.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Product> Products { get; set; } = default!;
    public IList<Category> Categories { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? CategoryId { get; set; }

    [BindProperty(SupportsGet = true)]
    public decimal? MinPrice { get; set; }

    [BindProperty(SupportsGet = true)]
    public decimal? MaxPrice { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SortOrder { get; set; }

    public async Task OnGetAsync()
    {
        Categories = await _context.Categories.ToListAsync();

        var productsQuery = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrEmpty(SearchString))
        {
            productsQuery = productsQuery.Where(p => p.Name.Contains(SearchString) || p.Description.Contains(SearchString));
        }

        if (CategoryId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.CategoryId == CategoryId);
        }

        if (MinPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price >= MinPrice.Value);
        }

        if (MaxPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price <= MaxPrice.Value);
        }

        switch (SortOrder)
        {
            case "price_asc":
                productsQuery = productsQuery.OrderBy(p => p.Price);
                break;
            case "price_desc":
                productsQuery = productsQuery.OrderByDescending(p => p.Price);
                break;
            case "name_desc":
                productsQuery = productsQuery.OrderByDescending(p => p.Name);
                break;
            default:
                productsQuery = productsQuery.OrderBy(p => p.Name);
                break;
        }

        Products = await productsQuery.ToListAsync();
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
