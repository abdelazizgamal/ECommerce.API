using ECommerce.BLL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL.Seed;

public static class SeedRunner
{
    public static async Task RunAsync(DbContext context)
    {
        // 🔹 Categories
        if (!await context.Set<Category>().AnyAsync())
        {
            var categories = SeedDataProvider.GetCategories();
            await context.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // 🔹 Products
        if (!await context.Set<Product>().AnyAsync())
        {
            var categories = await context.Set<Category>()
                .ToDictionaryAsync(c => c.Name, c => c.Id);

            var products = SeedDataProvider.GetProducts()
                .Select(p => new Product
                {
                    Title = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl,
                    CategoryId = categories[p.CategoryName], // 🔥 mapping
                    CreatedAt = p.CreatedAt
                });

            await context.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}