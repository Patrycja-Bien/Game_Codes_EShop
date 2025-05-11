using EShop.Domain.Repositories;
using EShopDomain.Models;
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
                new Category { Name = "Game_Category" },
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
        if (!context.Products.Any())
        {
            var category = await context.Categories
                    .Where(x => x.Name == "Game_Category").FirstOrDefaultAsync();

            var products = new List<Product>
            {
                new Product { Name = "Game_A", Category = category },
                new Product { Name = "Game_B", Category = category },
                new Product { Name = "Game_C", Category = category }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
