using EShop.Domain.Models;
using ProductCatalogue.Domain.DTOs;

namespace EShop.Application.Services;

public interface IProductService
{
    public Task<List<Product>> GetAllAsync();
    Task<Product> GetAsync(int id);
    Task<Product> UpdateAsync(Product product);
    Task<Product> AddAsync(Product product);
    Product Add(Product product);
    public Task<ProductDto> IsProductValidToProcessAsync(int productId, int quantity);

}
