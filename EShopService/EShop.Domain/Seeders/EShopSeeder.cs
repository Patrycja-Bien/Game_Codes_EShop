using EShop.Domain.Repositories;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Seeders;

public class EShopSeeder(DataContext context) : IEShopSeeder
{
    public async Task Seed()
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Seeder_Game_Category" },
                new Category { Name = "Horror" },
                new Category { Name = "Action" },
                new Category { Name = "Platformer" },
                new Category { Name = "Strategy" },
                new Category { Name = "Shooter" },
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
        if (!context.Products.Any())
        {
            var category = await context.Categories
                    .Where(x => x.Name == "Seeder_Game_Category").FirstOrDefaultAsync();

            var products = new List<Product>
            {
                new Product { Name = "Seeder_Game_A", Category = category, Sku = "Sku123" },
                new Product { Name = "Seeder_Game_B", Category = category, Sku = "Sku456" },
                new Product { Name = "Seeder_Game_C", Category = category, Sku = "Sku789" }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
