using Orders.Domain.Repositories;
using Orders.Domain.Models;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Orders.Domain.Seeders;

public class OrdersSeeder : IOrdersSeeder
{
    private readonly Orders.Domain.Repositories.DataContext _ordersContext;
    private readonly EShop.Domain.Repositories.DataContext _eshopContext;

    public OrdersSeeder(
        Orders.Domain.Repositories.DataContext ordersContext,
        EShop.Domain.Repositories.DataContext eshopContext)
    {
        _ordersContext = ordersContext;
        _eshopContext = eshopContext;
    }
    public async Task Seed()
    {
        await _ordersContext.Database.MigrateAsync();
        await _eshopContext.Database.MigrateAsync();

        if (!_ordersContext.Orders.Any())
        {
            var address1 = new Address { FirstName = "John", LastName = "Doe", Street = "123 Main St", City = "CityA", ZipCode = "12345", Country = "PL", Phone = "123456789", Email = "john@example.com" };
            var address2 = new Address { FirstName = "Jane", LastName = "Smith", Street = "456 Side St", City = "CityB", ZipCode = "67890", Country = "PL", Phone = "987654321", Email = "jane@example.com" };
            var address3 = new Address { FirstName = "Alice", LastName = "Brown", Street = "789 High St", City = "CityC", ZipCode = "54321", Country = "PL", Phone = "555555555", Email = "alice@example.com" };

            //var cartitems = await _eshopContext.CartItems.ToListAsync();

            //var orders = new List<Order>
            //{
            //    new Order { CartItems = cartitems, TotalAmount = 101, Address = address1 },
            //    new Order { CartItems = cartitems, TotalAmount = 102, Address = address2 },
            //    new Order { CartItems = cartitems, TotalAmount = 103, Address = address3 }
            //};

            //_ordersContext.Orders.AddRange(orders);
            //await _ordersContext.SaveChangesAsync();
        }

    }
}
