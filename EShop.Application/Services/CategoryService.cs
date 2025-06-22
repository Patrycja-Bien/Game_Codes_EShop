using EShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EShop.Domain.Repositories;

namespace EShop.Application.Services;

public class CategoryService : ICategoryService
{
    private IRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IDatabase _redisDb;


    public CategoryService(IRepository repository, IMemoryCache cache)
    {
        ConfigurationOptions conf = new ConfigurationOptions
        {
            EndPoints = { "redis:6379" },
            AbortOnConnectFail = false,
            ConnectTimeout = 60000
        };
        _repository = repository;
        _cache = cache;
        var redis = ConnectionMultiplexer.Connect(conf);
        _redisDb = redis.GetDatabase();
    }

    public async Task<List<Category>> GetAllAsync()
    {
        var result = await _repository.GetAllCategoriesAsync();
        foreach (Category category in result)
        {
            var key = $"Category:{category.Id}";
            var categoryJson = await _redisDb.StringGetAsync(key);
            if (string.IsNullOrEmpty(categoryJson))
            {
                await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(category), TimeSpan.FromHours(24));
            }
        }
        return result;
    }

    public async Task<Category> GetAsync(int id)
    {
        string key = $"Category:{id}";
        var categoryJson = await _redisDb.StringGetAsync(key);

        if (string.IsNullOrEmpty(categoryJson))
        {
            var category = await _repository.GetCategoryAsync(id);
            await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(category), TimeSpan.FromHours(24));
            return category;
        }
        else
        {
            var category = JsonSerializer.Deserialize<Category>(categoryJson);
            return category;
        }
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        var result = await _repository.UpdateCategoryAsync(category);

        string key = $"Category:{category.Id}";
        await _redisDb.KeyDeleteAsync(key);
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(category), TimeSpan.FromHours(24));

        return result;
    }

    public async Task<Category> AddAsync(Category category)
    {
        var result = await _repository.AddCategoryAsync(category);
        string key = $"Category:{category.Id}";
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(category), TimeSpan.FromHours(24));
        return result;
    }

}
