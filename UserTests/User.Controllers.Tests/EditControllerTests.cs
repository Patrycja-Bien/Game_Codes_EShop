using Moq;
using Moq.Language.Flow;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using UserService.Controllers;
using User.Application.Services;
using User.Domain.Models;
using User.Domain.Models.Response;

namespace User.Controllers.Tests;

public class EditControllerTests
{
    private readonly Mock<IEditUserService> _mockEditService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly EditController _controller;

    public EditControllerTests()
    {
        _mockEditService = new Mock<IEditUserService>();
        _mockUserService = new Mock<IUserService>();
        _controller = new EditController(_mockEditService.Object, _mockUserService.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task EditUserData_ShouldReturnUpdatedUsername()
    {
        // Arrange
        var userDto = new UserResponseDto { Id = 1, Email = "test@example.com" };
        var updatedUser = new User.Domain.Models.User { Id = 1, Username = "newName", Email = "test@example.com", CreatedAt = DateTime.UtcNow };

        _mockUserService.Setup(s => s.GetUserDataAsync(1)).ReturnsAsync(userDto);
        _mockEditService.Setup(s => s.EditUsernameAsync(It.IsAny<User.Domain.Models.User>())).ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.EditUserData("newName", "password");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDto = Assert.IsType<UserResponseDto>(okResult.Value);
        Assert.Equal("newName", returnedDto.Username);
    }

    [Fact]
    public async Task EditUserFullName_ShouldReturnUpdatedFullName()
    {
        // Arrange
        var userDto = new UserResponseDto { Id = 1, Username = "test", Email = "test@example.com" };
        var updatedUser = new User.Domain.Models.User { Id = 1, FullName = "New Full Name", CreatedAt = DateTime.UtcNow };

        _mockUserService.Setup(s => s.GetUserDataAsync(1)).ReturnsAsync(userDto);
        _mockEditService.Setup(s => s.EditFullNameAsync(It.IsAny<User.Domain.Models.User>())).ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.EditUserFullName("New Full Name", "password");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDto = Assert.IsType<UserResponseDto>(okResult.Value);
    }

    [Fact]
    public async Task EditUserEmail_ShouldReturnUpdatedEmail()
    {
        // Arrange
        var userDto = new UserResponseDto { Id = 1, Username = "test", Email = "old@example.com" };
        var updatedUser = new User.Domain.Models.User { Id = 1, Email = "new@example.com", CreatedAt = DateTime.UtcNow };

        _mockUserService.Setup(s => s.GetUserDataAsync(1)).ReturnsAsync(userDto);
        _mockEditService.Setup(s => s.EditEmailAsync(It.IsAny<User.Domain.Models.User>())).ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.EditUserEmail("new@example.com", "password");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDto = Assert.IsType<UserResponseDto>(okResult.Value);
        Assert.Equal("new@example.com", returnedDto.Email);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnOk_WhenPasswordChangesSuccessfully()
    {
        // Arrange  
        _mockEditService
            .Setup(s => s.ChangePasswordAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new User.Domain.Models.User { Id = 1, Username = "test", Email = "test@example.com", CreatedAt = DateTime.UtcNow });

        // Act  
        var result = await _controller.ResetPassword("oldPass", "newPass");

        // Assert  
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("changed successfully", okResult.Value.ToString());
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnNotFound_WhenChangeFails()
    {
        // Arrange
        _mockEditService
            .Setup(s => s.ChangePasswordAsync(1, "wrongOld", "newPass"))
            .ReturnsAsync((User.Domain.Models.User)null);

        // Act
        var result = await _controller.ResetPassword("wrongOld", "newPass");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("failed", notFoundResult.Value?.ToString() ?? string.Empty);
    }

    [Fact]
    public async Task EditMethods_ShouldReturnUnauthorized_WhenUserIdMissing()
    {
        // Arrange
        _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

        // Act
        var usernameResult = await _controller.EditUserData("newUsername", "password");
        var fullNameResult = await _controller.EditUserFullName("name", "pass");
        var emailResult = await _controller.EditUserEmail("email@test.com", "pass");
        var passwordResult = await _controller.ResetPassword("old", "new");

        // Assert
        Assert.IsType<UnauthorizedResult>(usernameResult.Result);
        Assert.IsType<UnauthorizedResult>(fullNameResult.Result);
        Assert.IsType<UnauthorizedResult>(emailResult.Result);
        Assert.IsType<UnauthorizedResult>(passwordResult);
    }
}
