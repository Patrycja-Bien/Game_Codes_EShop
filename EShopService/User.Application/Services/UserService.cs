using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Repositories;
using AutoMapper;
using User.Domain.Models.Response;
using System.Text.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Memory;

namespace User.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IDatabase _redisDb;
    private readonly IMemoryCache _cache;

    public UserService(IRepository userRepository, IMapper mapper, IMemoryCache cache)
    {
        ConfigurationOptions conf = new ConfigurationOptions
        {
            EndPoints = { "redis:6379" },
            AbortOnConnectFail = false,
            ConnectTimeout = 60000
        };
        _userRepository = userRepository;
        _cache = cache;
        var redis = ConnectionMultiplexer.Connect(conf);
        _redisDb = redis.GetDatabase();
    }

    public async Task<UserResponseDto?> GetUserDataAsync(int userId)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (user == null)
            return null;

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<User.Domain.Models.User> AddUserAsync(User.Domain.Models.User user)
    {
        var result = await _userRepository.AddUserAsync(user);
        string key = $"User:{user.Id}";
        await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(user), TimeSpan.FromHours(24));
        return result;
    }
}
