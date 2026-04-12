using ECommerce.Common;

namespace ECommerce.BLL.Entities;

public class Order : BaseEntity<int>, IAuditableEntity  
{

    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }


    // Shipping Address (Snapshot)
    public string ShippingCountry { get; set; }
    public string ShippingCity { get; set; }

    // Navigation
    public string UserId { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();


    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}