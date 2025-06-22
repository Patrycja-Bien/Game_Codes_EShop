namespace Orders.Application.Services;
using Orders.Domain.Models;

public interface IOrdersService
{
    public Task<List<Order>> GetAllAsync();
    public Task<Order> GetAsync(int id);
    public Task<Order> UpdateAsync(Order order);
    public Task<Order> AddAsync(Order order);
    public Order Add(Order order);
}
