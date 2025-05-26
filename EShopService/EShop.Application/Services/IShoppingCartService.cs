using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services;

public interface IShoppingCartService
{
    Task<ShoppingCart> GetCartAsync(int userId);
    Task AddItemAsync(int userId, int productId, int quantity);
    Task RemoveItemAsync(int userId, int productId);
    Task ClearCartAsync(int userId);
}
