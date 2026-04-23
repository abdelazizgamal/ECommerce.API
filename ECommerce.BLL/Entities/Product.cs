using ECommerce.Common;

namespace ECommerce.BLL.Entities;
public class Product : BaseEntity<int>, IAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }

    public string? ImageUrl { get; set; }

    // FK
    public int CategoryId { get; set; }

    // Navigation
    public Category Category { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}