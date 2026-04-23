using ECommerce.Common;

namespace ECommerce.BLL.Entities;

public class OrderItem : BaseEntity<int>, IAuditableEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPriceAtPurchase { get; set; }

    // Navigation
    public Order Order { get; set; }
    public Product Product { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}