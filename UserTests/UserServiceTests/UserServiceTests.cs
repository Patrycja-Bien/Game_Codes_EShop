using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using StackExchange.Redis;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using User.Application.Services;
using User.Domain.Models;
using User.Domain.Repositories;
using User.Domain.Models.Response;

namespace User.Application.Tests;

public class UserServiceTests
{
    private readonly Mock<IRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly Mock<IDatabase> _redisDbMock;
    private readonly Services.UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IRepository>();
        _mapperMock = new Mock<IMapper>();
        _memoryCacheMock = new Mock<IMemoryCache>();
        _redisDbMock = new Mock<IDatabase>();

        _redisDbMock
            .Setup(db => db.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        _userService = new Services.UserService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _memoryCacheMock.Object,
            _redisDbMock.Object
        );
    }

    [Fact]
    public async Task GetUserDataAsync_UserExists_ReturnsUserResponseDto()
    {
        var user = new Domain.Models.User { Id = 1, Username = "testuser" };
        var userResponseDto = new UserResponseDto { Username = "testuser" };

        _userRepositoryMock.Setup(r => r.GetUserAsync(1)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(userResponseDto);

        var result = await _userService.GetUserDataAsync(1);

        Assert.NotNull(result);
        Assert.Equal(userResponseDto.Username, result.Username);
    }

    [Fact]
    public async Task GetUserDataAsync_UserDoesNotExist_ReturnsNull()
    {
        _userRepositoryMock.Setup(r => r.GetUserAsync(1)).ReturnsAsync((Domain.Models.User)null);

        var result = await _userService.GetUserDataAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllUsersDataAsync_UsersExist_ReturnsListOfUserResponseDto()
    {
        var users = new List<Domain.Models.User>
        {
            new Domain.Models.User { Id = 1, Username = "user1" },
            new Domain.Models.User { Id = 2, Username = "user2" }
        };

        var dtos = new List<UserResponseDto>
        {
            new UserResponseDto { Username = "user1" },
            new UserResponseDto { Username = "user2" }
        };

        _userRepositoryMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(users[0])).Returns(dtos[0]);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(users[1])).Returns(dtos[1]);

        var result = await _userService.GetAllUsersDataAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("user1", result[0]?.Username);
        Assert.Equal("user2", result[1]?.Username);
    }

    [Fact]
    public async Task GetAllUsersDataAsync_NoUsers_ReturnsNull()
    {
        _userRepositoryMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync((List<Domain.Models.User>)null);

        var result = await _userService.GetAllUsersDataAsync();

        Assert.Null(result);
    }

    [Fact]
    public async Task AddUserAsync_AddsUser_ReturnsUserWithId()
    {
        var mockRepo = new Mock<IRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockCache = new Mock<IMemoryCache>();
        var mockRedisDb = new Mock<IDatabase>();

        var inputUser = new Domain.Models.User { Username = "newuser" };
        var savedUser = new Domain.Models.User { Id = 1, Username = "newuser" };

        mockRepo.Setup(r => r.AddUserAsync(It.IsAny<Domain.Models.User>()))
                .ReturnsAsync(savedUser);

        var userService = new Services.UserService(mockRepo.Object, mockMapper.Object, mockCache.Object, mockRedisDb.Object);

        var result = await userService.AddUserAsync(inputUser);

        Assert.Equal(1, result.Id);
    }
}
