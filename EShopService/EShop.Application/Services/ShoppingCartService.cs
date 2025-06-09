using EShop.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EShop.Domain.Repositories;
using StackExchange.Redis;

namespace EShop.Application.Services;

public class ShoppingCartService : IShoppingCartService
{
    private IRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IDatabase _redisDb;

    public ShoppingCartService(IRepository repository, IMemoryCache cache)
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

    private string GetCartKey(int userId) => $"ShoppingCart:{userId}";

    public async Task<ShoppingCart> GetCartAsync(int userId)
    {
        var key = GetCartKey(userId);
        var cartJson = await _redisDb.StringGetAsync(key);
        if (cartJson.IsNullOrEmpty)
            return new ShoppingCart { UserId = userId, Items = new List<CartItem>() };
        return JsonSerializer.Deserialize<ShoppingCart>(cartJson);
    }

    public async Task AddItemAsync(int userId, int productId, int quantity)
    {
        var cart = await GetCartAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        var product = await _repository.GetProductAsync(productId);
        if (product == null)
            throw new ArgumentException("Product not found");

        if (product.Stock < quantity)
            throw new InvalidOperationException("Not enough stock available");

        if (item != null)
        {
            if (product.Stock < item.Quantity + quantity)
                throw new InvalidOperationException("Not enough stock available for the requested quantity");

            item.Quantity += quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                Price = product.Price
            });
        }

        var key = GetCartKey(userId);
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(cart), TimeSpan.FromHours(24));
    }


    public async Task RemoveItemAsync(int userId, int productId)
    {
        var cart = await GetCartAsync(userId);
        var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (itemToRemove != null)
        {
            cart.Items.Remove(itemToRemove);
        }

        var key = GetCartKey(userId);
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(cart));
    }

    public async Task ClearCartAsync(int userId)
    {
        var key = GetCartKey(userId);
        await _redisDb.KeyDeleteAsync(key);
    }
}
