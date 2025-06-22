using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services;

public interface ICategoryService
{
    public Task<List<Category>> GetAllAsync();
    Task<Category> GetAsync(int id);
    Task<Category> UpdateAsync(Category category);
    Task<Category> AddAsync(Category category);
}
