using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextBuy.Data;
using NextBuy.Models;

namespace NextBuy.Areas.Admin.Pages.Products;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public EditModel(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty]
    public IFormFile? ImageUpload { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        Product = product;
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        var productToUpdate = await _context.Products.FindAsync(Product.Id);
        if (productToUpdate == null)
        {
            return NotFound();
        }

        productToUpdate.Name = Product.Name;
        productToUpdate.Description = Product.Description;
        productToUpdate.Price = Product.Price;
        productToUpdate.CategoryId = Product.CategoryId;

        if (ImageUpload != null)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(productToUpdate.ImageUrl))
            {
                 var oldImagePath = Path.Combine(_environment.WebRootPath, productToUpdate.ImageUrl.TrimStart('/'));
                 if (System.IO.File.Exists(oldImagePath))
                 {
                     System.IO.File.Delete(oldImagePath);
                 }
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageUpload.CopyToAsync(fileStream);
            }

            productToUpdate.ImageUrl = "/images/products/" + fileName;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(Product.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
