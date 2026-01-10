using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextBuy.Models;

public class Product
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Nom")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Description")]
    public string? Description { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Prix")]
    public decimal Price { get; set; }
    
    [Display(Name = "Image")]
    public string? ImageUrl { get; set; }
    
    [Display(Name = "Catégorie")]
    public int CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
}
