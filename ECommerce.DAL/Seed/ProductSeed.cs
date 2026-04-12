namespace ECommerce.DAL.Seed;

public class ProductSeed
{
    public string Name { get; set; }
    public string Description { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }

    public string ImageUrl { get; set; }

    public string CategoryName { get; set; } // 🔥 بدل CategoryId

    public DateTime CreatedAt { get; set; }
}