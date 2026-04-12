using ECommerce.BLL.Entities;

namespace ECommerce.DAL.Seed;

public static class SeedDataProvider
{
    public static List<Category> GetCategories()
    {
        var createdDate = new DateTime(2026, 3, 1, 10, 30, 0, DateTimeKind.Utc);

        return new List<Category>
{
            new() { Name = "Electronics", Description = "Devices and gadgets", CreatedAt = createdDate },
            new() { Name = "Clothing", Description = "Men and women fashion", CreatedAt = createdDate },
            new() { Name = "Home Appliances", Description = "Home devices", CreatedAt = createdDate },
            new() { Name = "Books", Description = "Educational and novels", CreatedAt = createdDate },
            new() { Name = "Sports", Description = "Sports equipment", CreatedAt = createdDate },
            new() { Name = "Toys", Description = "Kids toys", CreatedAt = createdDate }
        };
    }

    public static List<ProductSeed> GetProducts()
    {
        var createdDate = new DateTime(2026, 3, 1, 10, 30, 0, DateTimeKind.Utc);

        return new List<ProductSeed>
        {
            new()
            {
                Name = "Samsung Galaxy S24",
                Description = "Android flagship phone",
                Price = 52000,
                Stock = 12,
                ImageUrl = "",
                CategoryName = "Electronics",
                CreatedAt = createdDate
            },
            new()
            {
                Name = "MacBook Pro",
                Description = "High performance laptop",
                Price = 98000,
                Stock = 4,
                ImageUrl = "",
                CategoryName = "Electronics",
                CreatedAt = createdDate
            },
            new()
            {
                Name = "T-Shirt",
                Description = "Cotton t-shirt",
                Price = 450,
                Stock = 50,
                ImageUrl = "",
                CategoryName = "Clothing",
                CreatedAt = createdDate
            },
            new()
            {
                Name = "Air Fryer",
                Description = "Healthy cooking",
                Price = 3700,
                Stock = 10,
                ImageUrl = "",
                CategoryName = "Home Appliances",
                CreatedAt = createdDate
            },
            new()
            {
                Name = "Clean Code",
                Description = "Programming book",
                Price = 650,
                Stock = 20,
                ImageUrl = "",
                CategoryName = "Books",
                CreatedAt = createdDate
            },
            new()
            {
                Name = "Football",
                Description = "Professional ball",
                Price = 700,
                Stock = 30,
                ImageUrl = "",
                CategoryName = "Sports",
                CreatedAt = createdDate
            }
        };
    }
}