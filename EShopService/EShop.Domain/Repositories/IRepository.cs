using EShopDomain.Models;

namespace EShop.Domain.Repositories;

public interface IRepository
{
    #region Product
    Task<Product> GetProductAsync(int id);
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
    #endregion

}
