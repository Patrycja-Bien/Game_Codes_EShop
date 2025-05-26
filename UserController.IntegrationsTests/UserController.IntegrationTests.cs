using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using User.Application.Services;
using User.Domain.Models.Response;
using Xunit;
using UserService;

namespace UserController.IntegrationsTests;

public class UserControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UserControllerIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Optionally replace IUserService with a mock for more control
                var userServiceMock = new Mock<IUserService>();
                userServiceMock.Setup(s => s.GetUserDataAsync(It.IsAny<int>()))
                    .ReturnsAsync(new UserResponseDto
                    {
                        Id = 1,
                        Username = "testuser",
                        Email = "test@test.com"
                    });

                services.AddScoped(_ => userServiceMock.Object);
            });
        });
    }

    [Fact]
    public async Task GetUserData_Authorized_ReturnsUserData()
    {
        var client = _factory.CreateClient();

        // Simulate authentication
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dummy-token");

        var response = await client.GetAsync("/api/user/me");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponseDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(user);
        Assert.Equal("testuser", user.Username);
    }
}
