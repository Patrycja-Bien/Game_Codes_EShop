using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Domain.Models;

namespace Orders.Domain.Repositories;

public interface IRepository
{
    #region Order
    Task<Order> GetOrderAsync(int id);
    Task<Order> AddOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task<List<Order>> GetAllOrdersAsync();
    #endregion
}
