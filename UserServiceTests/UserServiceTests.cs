using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using User.Application.Services;
using User.Domain.Models;
using User.Domain.Models.Response;
using User.Domain.Repositories;
using Xunit;

public class UserServiceTests
{
    [Fact]
    public async Task GetUserDataAsync_UserExists_ReturnsUserResponseDto()
    {
        // Arrange  
        var userId = 1;
        var user = new User.Domain.Models.User { Id = userId, Username = "test", Email = "test@test.com" };
        var userResponse = new UserResponseDto { Id = userId, Username = "test", Email = "test@test.com" };

        var repoMock = new Mock<IRepository>();
        repoMock.Setup(r => r.GetUserAsync(userId)).ReturnsAsync(user);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<UserResponseDto>(user)).Returns(userResponse);

        var cacheMock = new Mock<IMemoryCache>();
        var service = new UserService(repoMock.Object, mapperMock.Object, cacheMock.Object);

        // Act  
        var result = await service.GetUserDataAsync(userId);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("test", result.Username);
    }

    [Fact]
    public async Task GetUserDataAsync_UserDoesNotExist_ReturnsNull()
    {
        // Arrange  
        var userId = 2;
        var repoMock = new Mock<IRepository>();
        repoMock.Setup(r => r.GetUserAsync(userId)).ReturnsAsync((User.Domain.Models.User?)null);

        var mapperMock = new Mock<IMapper>();
        var cacheMock = new Mock<IMemoryCache>();
        var service = new UserService(repoMock.Object, mapperMock.Object, cacheMock.Object);

        // Act  
        var result = await service.GetUserDataAsync(userId);

        // Assert  
        Assert.Null(result);
    }
}
