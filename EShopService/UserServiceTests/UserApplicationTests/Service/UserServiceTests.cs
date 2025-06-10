using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using User.Application.Services;
using User.Domain.Models;
using User.Domain.Models.DTOs;
using User.Domain.Models.Profiles;
using User.Domain.Models.Response;
using User.Domain.Repositories;
using Xunit;

namespace User.Application.Tests.Service;

public class UserServiceTest
{
    private readonly Mock<IRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<IDatabase> _redisDbMock;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheMock = new Mock<IMemoryCache>();
        _redisDbMock = new Mock<IDatabase>();
    }

    [Fact]
    public async Task GetUserDataAsync_ReturnsUserResponseDto_WhenUserExists()
    {
        // Arrange
        var user = new User.Domain.Models.User { Id = 1, Username = "test", Email = "test" };
        var dto = new UserResponseDto { Id = 1, Username = "test" };

        _userRepositoryMock.Setup(r => r.GetUserAsync(1)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(dto);

        var userService = new User.Application.Services.UserService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _cacheMock.Object
        );

        // Act
        var result = await userService.GetUserDataAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Id, result.Id);
        Assert.Equal(dto.Username, result.Username);
    }

    [Fact]
    public async Task GetUserDataAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        _userRepositoryMock.Setup(r => r.GetUserAsync(1)).ReturnsAsync((User.Domain.Models.User)null);

        var userService = new User.Application.Services.UserService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _cacheMock.Object
        );

        var result = await userService.GetUserDataAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllUsersDataAsync_ReturnsListOfUserResponseDto()
    {
        var users = new List<User.Domain.Models.User>
        {
            new User.Domain.Models.User { Id = 1, Username = "user1" },
            new User.Domain.Models.User { Id = 2, Username = "user2" }
        };
        var dtos = new List<UserResponseDto>
        {
            new UserResponseDto { Id = 1, Username = "user1" },
            new UserResponseDto { Id = 2, Username = "user2" }
        };

        _userRepositoryMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(users[0])).Returns(dtos[0]);
        _mapperMock.Setup(m => m.Map<UserResponseDto>(users[1])).Returns(dtos[1]);

        var userService = new User.Application.Services.UserService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _cacheMock.Object
        );

        var result = await userService.GetAllUsersDataAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("user1", result[0].Username);
        Assert.Equal("user2", result[1].Username);
    }

    [Fact]
    public async Task GetAllUsersDataAsync_ReturnsNull_WhenNoUsers()
    {
        _userRepositoryMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync((List<User.Domain.Models.User>)null);

        var userService = new User.Application.Services.UserService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _cacheMock.Object
        );

        var result = await userService.GetAllUsersDataAsync();

        Assert.Null(result);
    }
}

