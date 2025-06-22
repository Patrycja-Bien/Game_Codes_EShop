using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UserService.Controllers;
using User.Application.Services;
using User.Domain.Models.Response;
using User.Domain.Models.DTOs;

namespace User.Controllers.Tests;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockService = new Mock<IUserService>();
        _controller = new UserController(_mockService.Object);

        // Mock authenticated user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task GetUserData_ShouldReturnUserDto_WhenExists()
    {
        // Arrange
        var userDto = new UserResponseDto { Id = 1, Username = "Test User" };
        _mockService.Setup(s => s.GetUserDataAsync(1)).ReturnsAsync(userDto);

        // Act
        var result = await _controller.GetUserData();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(userDto, okResult.Value);
    }

    [Fact]
    public async Task GetUserData_ShouldReturnNotFound_WhenUserIsNull()
    {
        // Arrange
        _mockService.Setup(s => s.GetUserDataAsync(1)).ReturnsAsync((UserResponseDto)null);

        // Act
        var result = await _controller.GetUserData();

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllUsersData_ShouldReturnListOfUserDtos()
    {
        // Arrange
        var users = new List<UserResponseDto?>
        {
            new UserResponseDto { Id = 1 },
            new UserResponseDto { Id = 2 }
        };

        _mockService.Setup(s => s.GetAllUsersDataAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAllUsersData();

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<UserResponseDto?>>>(result);
        Assert.Equal(users, actionResult.Value);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedUser()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            UserName = "testuser",
            FullName = "Test User",
            Email = "test@example.com",
            Password = "Password123"
        };

        var createdUser = new User.Domain.Models.User
        {
            Id = 1,
            Username = createUserDto.UserName,
            FullName = createUserDto.FullName,
            Email = createUserDto.Email
        };

        _mockService.Setup(s => s.AddUserAsync(It.IsAny<User.Domain.Models.User>()))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _controller.CreateUser(createUserDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(createdUser, okResult.Value);
    }
}
