using ShoppingCart.Domain.Models;
using Item = ShoppingCart.Domain.Models.Item;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Domain.Seeders;

public class ShoppingCartSeeder(DataContext context) : IShoppingCartSeeder
{
    public async Task Seed()
    {
        {
            if (!context.Items.Any())
            {
                var items = new List<Item>
               {
                   new Item { Name = "Game_A", Price = 29.99M, Quantity = 1 },
                   new Item { Name = "Game_B", Price = 39.99M, Quantity = 1 },
                   new Item { Name = "Game_C", Price = 49.99M, Quantity = 1 },
                   new Item { Name = "Game_D", Price = 59.99M, Quantity = 1 },
                   new Item { Name = "Game_E", Price = 69.99M, Quantity = 1 },
                   new Item { Name = "Game_F", Price = 19.99M, Quantity = 1 },
               };

                context.Items.AddRange(items);
                context.SaveChanges();
            }
            if (!context.Carts.Any())
            {

                var carts = new List<Cart>
               {
                   new Cart {  },
                   new Cart {  },
                   new Cart {  }
               };

                context.Carts.AddRange(carts);
                context.SaveChanges();
            }
        }
    }
}
