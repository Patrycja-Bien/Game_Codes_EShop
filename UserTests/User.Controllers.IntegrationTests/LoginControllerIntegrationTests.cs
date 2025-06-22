using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using UserService;
using User.Domain.Models;
using User.Domain.Models.Requests;
using User.Domain.Repositories;

namespace User.Controllers.IntegrationTests;

public class LoginControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public LoginControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DataContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                db.Database.EnsureCreated();

                if (!db.Users.Any())
                {
                    db.Users.Add(new Domain.Models.User
                    {
                        Username = "testuser",
                        Email = "testuser@example.com",
                        PasswordHash = Domain.Helpers.PasswordHelper.Hash("password123"),
                        CreatedAt = DateTime.UtcNow,
                        Roles = new List<Role> { new Role { Name = "Administrator" } }
                    });
                    db.SaveChanges();
                }
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsAreValid()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "testuser",
            Password = "password123"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/login", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadAsStringAsync();
        Assert.Contains("token", responseData);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginRequest = new LoginRequest { Username = "wrong", Password = "wrong" };
        var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/login", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AdminPage_ReturnsOk_WhenTokenIsValid()
    {
        var loginRequest = new LoginRequest
        {
            Username = "testuser",
            Password = "password123"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");

        var loginResponse = await _client.PostAsync("/api/login", content);
        loginResponse.EnsureSuccessStatusCode();

        var json = await loginResponse.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        var token = doc.RootElement.GetProperty("token").GetString();

        // Act
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/login");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task AdminPage_ReturnsUnauthorized_WithoutToken()
    {
        // Act
        var response = await _client.GetAsync("/api/login");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
