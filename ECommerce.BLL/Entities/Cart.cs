using ECommerce.Common;

namespace ECommerce.BLL.Entities;

public class Cart : BaseEntity<int>, IAuditableEntity
{
    public string UserId { get; set; }


    // Navigation
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}