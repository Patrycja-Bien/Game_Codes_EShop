using Microsoft.EntityFrameworkCore;
using Orders.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Repositories;

public class Repository : IRepository
{
    private readonly DataContext _context;

    public Repository(DataContext dataContext)
    {
        _context = dataContext;
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> GetOrderAsync(int id)
    {
        return await _context.Orders.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
}
