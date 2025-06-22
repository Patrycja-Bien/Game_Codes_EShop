using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Orders.Domain.Models;
using Orders.Domain.Repositories;
using Order = Orders.Domain.Models.Order;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Orders.Application.Services;

public class OrdersService : IOrdersService
{
    private IRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IDatabase _redisDb;


    public OrdersService(IRepository repository, IMemoryCache cache)
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

    public async Task<List<Order>> GetAllAsync()
    {
        var result = await _repository.GetAllOrdersAsync();
        foreach (Order order in result)
        {
            var key = $"Order:{order.Id}";
            var orderJson = await _redisDb.StringGetAsync(key);
            if (string.IsNullOrEmpty(orderJson))
            {
                await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(order), TimeSpan.FromHours(24));
            }
        }
        return result;
    }

    public async Task<Order> GetAsync(int id)
    {
        string key = $"Order:{id}";
        var orderJson = await _redisDb.StringGetAsync(key);

        if (string.IsNullOrEmpty(orderJson))
        {
            var order = await _repository.GetOrderAsync(id);
            await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(order), TimeSpan.FromHours(24));
            return order;
        }
        else
        {
            var order = JsonSerializer.Deserialize<Order>(orderJson);
            return order;
        }
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        var result = await _repository.UpdateOrderAsync(order);

        string key = $"Order:{order.Id}";
        await _redisDb.KeyDeleteAsync(key);
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(order), TimeSpan.FromHours(24));

        return result;
    }

    public async Task<Order> AddAsync(Order order)
    {
        var result = await _repository.AddOrderAsync(order);
        string key = $"Order:{order.Id}";
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(order), TimeSpan.FromHours(24));
        return result;
    }

    public Order Add(Order order)
    {
        var result = _repository.AddOrderAsync(order).Result;
        string key = $"Order:{order.Id}";
        _redisDb.StringSetAsync(key, JsonSerializer.Serialize(order), TimeSpan.FromHours(24));
        return result;
    }
}
