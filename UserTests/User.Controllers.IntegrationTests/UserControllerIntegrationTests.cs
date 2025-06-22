using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Net;
using User.Domain.Models.DTOs;
using User.Domain.Models.Response;
using UserService;
using User.Domain.Repositories;
using User.Domain.Models;

namespace User.Controllers.IntegrationTests;

public class UserControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public UserControllerIntegrationTest(WebApplicationFactory<Program> factory)
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
    public async Task Post_CreateUser_ReturnsCreatedUser()
    {
        // Arrange
        var newUser = new CreateUserDto
        {
            Username = "Test",
            Fullname = "fullname",
            Email = "email@example.com",
            Password = "password"
        };

        var content = JsonContent.Create(newUser);

        // Act
        var response = await _client.PostAsync("/api/User/Create-User", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Error: " + error);
        }

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Test", responseString);
    }

    [Fact]
    public async Task Get_GetAllUsersData_ReturnsUserList()
    {
        // Act
        var response = await _client.GetAsync("/api/User/Get-All");

        // Assert
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>();
        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }

    [Fact]
    public async Task Get_GetUserData_UnauthorizedWithoutToken()
    {
        // Act
        var response = await _client.GetAsync("/api/User/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
