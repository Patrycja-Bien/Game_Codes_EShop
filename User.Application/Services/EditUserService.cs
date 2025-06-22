using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Exceptions.Login;
using User.Domain.Helpers;
using User.Domain.Repositories;

namespace User.Application.Services;

public class EditUserService : IEditUserService
{
    private readonly IRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IDatabase _redisDb;
    private readonly IMemoryCache _cache;

    public EditUserService(IRepository userRepository, IMapper mapper, IMemoryCache cache)
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
        _mapper = mapper;
    }
    public async Task<User.Domain.Models.User> EditUsernameAsync(User.Domain.Models.User user)
    {
        var curr_user = await _userRepository.GetUserAsync(user.Id);
        if (curr_user == null)
            throw new InvalidCredentialsException();
        curr_user.Username = user.Username;
        var result = await _userRepository.UpdateUserAsync(curr_user);
        return result;
    }
    public async Task<User.Domain.Models.User> EditFullNameAsync(User.Domain.Models.User user)
    {
        var curr_user = await _userRepository.GetUserAsync(user.Id);
        if (curr_user == null)
            throw new InvalidCredentialsException();
        curr_user.FullName = user.FullName;
        var result = await _userRepository.UpdateUserAsync(curr_user);
        return result;
    }

    public async Task<User.Domain.Models.User> EditEmailAsync(User.Domain.Models.User user)
    {
        var curr_user = await _userRepository.GetUserAsync(user.Id);
        if (curr_user == null)
            throw new InvalidCredentialsException();
        curr_user.Email = user.Email;
        var result = await _userRepository.UpdateUserAsync(curr_user);
        return result;
    }
    public async Task<User.Domain.Models.User> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (!PasswordHelper.Verify(oldPassword, user.PasswordHash))
            throw new InvalidCredentialsException();

        if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
        {
            throw new ArgumentException("Old password and new password cannot be null or empty.");
        }
        if (oldPassword == newPassword)
        {
            throw new ArgumentException("Old password and new password cannot be the same.");
        }
        user.PasswordHash = PasswordHelper.Hash(newPassword);
        var result = await _userRepository.UpdateUserAsync(user);
        return result;
    }
}
