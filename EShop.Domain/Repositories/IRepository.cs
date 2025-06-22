using EShop.Domain.Models;

namespace EShop.Domain.Repositories;

public interface IRepository
{
    #region Product
    Task<Product> GetProductAsync(int id);
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
    #endregion

    #region Category
    Task<Category> GetCategoryAsync(int id);
    Task<Category> AddCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task<List<Category>> GetAllCategoriesAsync();
    #endregion

}
