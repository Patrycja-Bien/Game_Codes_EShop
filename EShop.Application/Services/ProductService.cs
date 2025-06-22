using EShop.Domain.Repositories;
using EShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System.Text.Json;
using ProductCatalogue.Domain.DTOs;

namespace EShop.Application.Services;

public class ProductService : IProductService
{
    private IRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IDatabase _redisDb;


    public ProductService(IRepository repository, IMemoryCache cache)
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

    public async Task<List<Product>> GetAllAsync()
    {
        var result = await _repository.GetAllProductsAsync();
        foreach (Product product in result)
        {
            var key = $"Product:{product.Id}";
            var productJson = await _redisDb.StringGetAsync(key);
            if (string.IsNullOrEmpty(productJson))
            {
                await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(product), TimeSpan.FromHours(24));
            }
        }
        return result;
    }

    public async Task<Product> GetAsync(int id)
    {
        string key = $"Product:{id}";
        var productJson = await _redisDb.StringGetAsync(key);

        if (string.IsNullOrEmpty(productJson))
        {
            var product = await _repository.GetProductAsync(id);
            await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(product), TimeSpan.FromHours(24));
            return product;
        }
        else
        {
            var product = JsonSerializer.Deserialize<Product>(productJson);
            return product;
        }
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        var result = await _repository.UpdateProductAsync(product);

        string key = $"Product:{product.Id}";
        await _redisDb.KeyDeleteAsync(key);
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(product), TimeSpan.FromHours(24));

        return result;
    }

    public async Task<Product> AddAsync(Product product)
    {
        var result =  await _repository.AddProductAsync(product);
        string key = $"Product:{product.Id}";
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(product), TimeSpan.FromHours(24));
        return result;
    }

    public Product Add(Product product)
    {
        var result = _repository.AddProductAsync(product).Result;
        string key = $"Product:{product.Id}";
        _redisDb.StringSetAsync(key, JsonSerializer.Serialize(product), TimeSpan.FromHours(24));
        return result;
    }

    public async Task<ProductDto> IsProductValidToProcessAsync(int productId, int quantity)
    {
        var product = await _repository.GetProductAsync(productId);
        if (product == null)
            return new ProductDto { IsAvailable = false };
        if (product.Stock < quantity)
            return new ProductDto { IsAvailable = false };
        return new ProductDto { IsAvailable = true, Name = product.Name, Price = product.Price, Quantity = product.Stock - quantity };
    }

}
