using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextBuy.Models;

public class Order
{
    public int Id { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    
    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    public string Status { get; set; } = "En attente"; // En attente, Expédiée, Livrée
    
    public ICollection<OrderItem>? OrderItems { get; set; }
}
