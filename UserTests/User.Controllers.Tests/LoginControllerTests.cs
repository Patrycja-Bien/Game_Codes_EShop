using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UserService.Controllers;
using User.Application.Services;
using User.Domain.Exceptions.Login;
using User.Domain.Models.Requests;

namespace User.Controllers.Tests;

public class LoginControllerTests
{
    private readonly Mock<ILoginService> _mockLoginService;
    private readonly LoginController _controller;

    public LoginControllerTests()
    {
        _mockLoginService = new Mock<ILoginService>();
        _controller = new LoginController(_mockLoginService.Object);
    }

    [Fact]
    public void Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest { Username = "testuser", Password = "password123" };
        _mockLoginService.Setup(s => s.Login(request.Username, request.Password)).Returns("fake-jwt-token");

        // Act
        var result = _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.Contains("token", okResult.Value.ToString());
    }

    [Fact]
    public void Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new LoginRequest { Username = "invalid", Password = "wrong" };
        _mockLoginService.Setup(s => s.Login(request.Username, request.Password)).Throws(new InvalidCredentialsException());

        // Act
        var result = _controller.Login(request);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void AdminPage_ShouldReturnOk_WhenUserIsAuthorized()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = _controller.AdminPage();

        // Assert
        Assert.IsType<OkResult>(result);
    }
}
