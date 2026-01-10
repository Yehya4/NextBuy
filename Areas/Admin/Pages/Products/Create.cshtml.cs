using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NextBuy.Data;
using NextBuy.Models;

namespace NextBuy.Areas.Admin.Pages.Products;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public IActionResult OnGet()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return Page();
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty]
    public IFormFile? ImageUpload { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
             ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        if (ImageUpload != null)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageUpload.CopyToAsync(fileStream);
            }

            Product.ImageUrl = "/images/products/" + fileName;
        }

        _context.Products.Add(Product);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
