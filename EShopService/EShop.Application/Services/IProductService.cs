using EShopDomain.Models;

namespace EShop.Application.Services;

public interface IProductService
{
    public Task<List<Product>> GetAllAsync();
    Task<Product> GetAsync(int id);
    Task<Product> UpdateAsync(Product game);
    Task<Product> AddAsync(Product game);
    Product Add(Product game);
}
