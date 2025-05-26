using Microsoft.EntityFrameworkCore;
using EShop.Domain.Models;

namespace EShop.Domain.Repositories;

public class Repository : IRepository
{
    private readonly DataContext _context;

    public Repository(DataContext dataContext)
    {
        _context = dataContext;
    }

    #region Product
    public async Task<Product> AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetProductAsync(int id)
    {
        return await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }
    #endregion

    #region Category
    public async Task<Category> AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        return await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }
    #endregion
}
