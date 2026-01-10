using System.ComponentModel.DataAnnotations;

namespace NextBuy.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Nom")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Description")]
    public string? Description { get; set; }
    
    public ICollection<Product>? Products { get; set; }
}
