using ECommerce.Common;

namespace ECommerce.BLL.Entities;

public class Category : BaseEntity<int>, IAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }

    // Navigation
    public ICollection<Product> Products { get; set; } = new List<Product>();


    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}