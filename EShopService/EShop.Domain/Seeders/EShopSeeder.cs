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
                new Product { Name = "Seeder_Game_A", Category = category, Sku = "Sku123", Stock = 10 },
                new Product { Name = "Seeder_Game_B", Category = category, Sku = "Sku456", Stock = 10 },
                new Product { Name = "Seeder_Game_C", Category = category, Sku = "Sku789", Stock = 10 }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
        if (!context.ShoppingCarts.Any())
        {
            var cartitems1 = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Seeder_Game_A", Price = 99.99m, Sku = "Sku123", Quantity = 1 },
                new CartItem { ProductId = 2, Name = "Seeder_Game_B", Price = 29.99m, Sku = "Sku456", Quantity = 2 }
            };
            var cartitems2 = new List<CartItem>
            {
                new CartItem { ProductId = 1, Name = "Seeder_Game_A", Price = 19.99m, Sku = "Sku123", Quantity = 1 },
                new CartItem { ProductId = 3, Name = "Seeder_Game_C", Price = 399.99m, Sku = "Sku789", Quantity = 1 }
            };
            var cartitems3 = new List<CartItem>
            {
                new CartItem { ProductId = 2, Name = "Seeder_Game_B", Price = 29.99m, Sku = "Sku456", Quantity = 1 },
                new CartItem { ProductId = 3, Name = "Seeder_Game_C", Price = 399.99m, Sku = "Sku789", Quantity = 2 }
            };
            var shoppingcarts = new List<ShoppingCart>
            {
                new ShoppingCart { UserId = 1, Items = cartitems1 },
                new ShoppingCart { UserId = 2, Items = cartitems2 },
                new ShoppingCart { UserId = 3, Items = cartitems3 }
            };
        }
    }
}
