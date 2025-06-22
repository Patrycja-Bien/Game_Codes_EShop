using Xunit;
using Moq;
using System.Collections.Generic;
using User.Application.Services;
using User.Domain.Repositories;
using User.Domain.Exceptions.Login;
using User.Application.Producer;
using User.Domain.Models;
using System.Linq;

namespace User.Application.Tests;
public class LoginServiceTests
{
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();
    private readonly Mock<IRepository> _userRepositoryMock = new();
    private readonly Mock<IKafkaProducer> _kafkaProducerMock = new();
    private readonly Queue<int> _userLoggedIdsQueue = new();

    private LoginService CreateService()
    {
        return new LoginService(
            _jwtTokenServiceMock.Object,
            _userLoggedIdsQueue,
            _userRepositoryMock.Object,
            _kafkaProducerMock.Object);
    }

    [Fact]
    public void Login_WithInvalidUsername_ThrowsInvalidCredentialsException()
    {
        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Models.User?)null);

        var service = CreateService();

        Assert.Throws<InvalidCredentialsException>(() => service.Login("nonexistent", "password"));
    }

    [Fact]
    public void Login_WithInvalidPassword_ThrowsInvalidCredentialsException()
    {
        var validHash = User.Domain.Helpers.PasswordHelper.Hash("correctpassword");

        var user = new User.Domain.Models.User
        {
            Id = 1,
            Username = "user1",
            PasswordHash = validHash,
            Roles = new List<Role>()
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("user1"))
            .ReturnsAsync(user);

        var service = CreateService();

        Assert.Throws<InvalidCredentialsException>(() => service.Login("user1", "wrongpassword"));
    }

    [Fact]
    public void Login_WithValidCredentials_ReturnsTokenAndUpdatesQueueAndSendsMessage()
    {
        var user = new Domain.Models.User  
        {
            Username = "validUser",
            PasswordHash = Domain.Helpers.PasswordHelper.Hash("correctpassword"),
            Email = "user@example.com",
            Roles = new List<Role> { new() { Name = "Admin" } }
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync("validUser"))
            .ReturnsAsync(user);

        _jwtTokenServiceMock.Setup(j => j.GenerateToken(user.Id, It.IsAny<List<string>>()))
            .Returns("valid.jwt.token");

        var service = CreateService();

        var token = service.Login("validUser", "correctpassword");

        Assert.Equal("valid.jwt.token", token);
        Assert.Contains(user.Id, _userLoggedIdsQueue);
        _kafkaProducerMock.Verify(k => k.SendMessageAsync("after-login-email-topic", user.Email), Times.Once);
    }
}
