using ECommerce.Common;

namespace ECommerce.BLL.Entities;

public class CartItem : BaseEntity<int>, IAuditableEntity
{
    public int CartId { get; set; }
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    // Navigation
    public Cart Cart { get; set; }
    public Product Product { get; set; }
    public DateTime CreatedAt { get ; set; }
    public DateTime? UpdatedAt { get ; set ; }
}