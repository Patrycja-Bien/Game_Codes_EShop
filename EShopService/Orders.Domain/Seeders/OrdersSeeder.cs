using Orders.Domain.Repositories;
using Orders.Domain.Models;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Orders.Domain.Seeders;

public class OrdersSeeder(DataContext context) : IOrdersSeeder
{
    public async Task Seed()
    {
        await context.Database.MigrateAsync();
        if (!context.Orders.Any())
        {
            var address1 = new Address { FirstName = "John", LastName = "Doe", Street = "123 Main St", City = "CityA", ZipCode = "12345", Country = "PL", Phone = "123456789", Email = "john@example.com" };
            var address2 = new Address { FirstName = "Jane", LastName = "Smith", Street = "456 Side St", City = "CityB", ZipCode = "67890", Country = "PL", Phone = "987654321", Email = "jane@example.com" };
            var address3 = new Address { FirstName = "Alice", LastName = "Brown", Street = "789 High St", City = "CityC", ZipCode = "54321", Country = "PL", Phone = "555555555", Email = "alice@example.com" };

            var products1 = new List<Product> { /* ... */ };
            var products2 = new List<Product> { /* ... */ };
            var products3 = new List<Product> { /* ... */ };

            var orders = new List<Order>
    {
        new Order { Products = products1, TotalAmount = 101, Address = address1 },
        new Order { Products = products2, TotalAmount = 102, Address = address2 },
        new Order { Products = products3, TotalAmount = 103, Address = address3 }
    };

            context.Orders.AddRange(orders);
            await context.SaveChangesAsync();
        }

    }
}
