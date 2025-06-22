using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Threading.Tasks;
using User.Application.Services;
using User.Domain.Exceptions.Login;
using User.Domain.Helpers;
using User.Domain.Models;
using User.Domain.Repositories;
using Xunit;

namespace User.Application.Tests;

public class EditUserServiceTests
{
    private readonly Mock<IRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

    private readonly EditUserService _service;

    public EditUserServiceTests()
    {
        _service = new EditUserService(_userRepositoryMock.Object, _mapperMock.Object, _memoryCache);
    }

    [Fact]
    public async Task EditUsernameAsync_ValidUser_UpdatesUsername()
    {
        var user = new Domain.Models.User { Id = 1, Username = "newuser" };
        var existingUser = new Domain.Models.User { Id = 1, Username = "olduser" };

        _userRepositoryMock.Setup(r => r.GetUserAsync(user.Id)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(r => r.UpdateUserAsync(It.IsAny<Domain.Models.User>())).ReturnsAsync(user);

        var result = await _service.EditUsernameAsync(user);

        Assert.Equal("newuser", result.Username);
        _userRepositoryMock.Verify(r => r.UpdateUserAsync(It.Is<Domain.Models.User>(u => u.Username == "newuser")), Times.Once);
    }

    [Fact]
    public async Task EditFullNameAsync_ValidUser_UpdatesFullName()
    {
        var user = new Domain.Models.User { Id = 1, FullName = "New Name" };
        var existingUser = new Domain.Models.User { Id = 1, FullName = "Old Name" };

        _userRepositoryMock.Setup(r => r.GetUserAsync(user.Id)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(r => r.UpdateUserAsync(It.IsAny<Domain.Models.User>())).ReturnsAsync(user);

        var result = await _service.EditFullNameAsync(user);

        Assert.Equal("New Name", result.FullName);
    }

    [Fact]
    public async Task EditEmailAsync_ValidUser_UpdatesEmail()
    {
        var user = new Domain.Models.User { Id = 1, Email = "new@example.com" };
        var existingUser = new Domain.Models.User { Id = 1, Email = "old@example.com" };

        _userRepositoryMock.Setup(r => r.GetUserAsync(user.Id)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(r => r.UpdateUserAsync(It.IsAny<Domain.Models.User>())).ReturnsAsync(user);

        var result = await _service.EditEmailAsync(user);

        Assert.Equal("new@example.com", result.Email);
    }

    [Fact]
    public async Task ChangePasswordAsync_ValidInput_ChangesPassword()
    {
        var userId = 1;
        var oldPassword = "oldPass";
        var newPassword = "newPass";
        var hashedOldPassword = PasswordHelper.Hash(oldPassword);

        var user = new Domain.Models.User { Id = userId, PasswordHash = hashedOldPassword };

        _userRepositoryMock.Setup(r => r.GetUserAsync(userId)).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.UpdateUserAsync(It.IsAny<Domain.Models.User>())).ReturnsAsync(user);

        var result = await _service.ChangePasswordAsync(userId, oldPassword, newPassword);

        Assert.True(PasswordHelper.Verify(newPassword, result.PasswordHash));
    }

    [Fact]
    public async Task ChangePasswordAsync_WrongOldPassword_ThrowsInvalidCredentials()
    {
        var user = new Domain.Models.User
        {
            Id = 1,
            PasswordHash = PasswordHelper.Hash("correctOld")
        };

        _userRepositoryMock.Setup(r => r.GetUserAsync(user.Id)).ReturnsAsync(user);

        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _service.ChangePasswordAsync(user.Id, "wrongOld", "newPassword"));
    }

    [Fact]
    public async Task ChangePasswordAsync_SameOldAndNew_ThrowsArgumentException()
    {
        var password = "samePass";
        var user = new Domain.Models.User
        {
            Id = 1,
            PasswordHash = PasswordHelper.Hash(password)
        };

        _userRepositoryMock.Setup(r => r.GetUserAsync(user.Id)).ReturnsAsync(user);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ChangePasswordAsync(user.Id, password, password));

        Assert.Equal("Old password and new password cannot be the same.", ex.Message);
    }

    [Fact]
    public async Task ChangePasswordAsync_NullPassword_ThrowsArgumentNullException()
    {
        var user = new Domain.Models.User
        {
            Id = 1,
            PasswordHash = PasswordHelper.Hash("anything")
        };

        _userRepositoryMock.Setup(r => r.GetUserAsync(user.Id)).ReturnsAsync(user);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.ChangePasswordAsync(user.Id, null, null));
    }
}
