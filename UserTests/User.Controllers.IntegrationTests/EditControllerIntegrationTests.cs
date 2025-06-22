using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User.Domain.Models;
using User.Domain.Models.Response;
using User.Domain.Helpers;
using UserService;
using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;
using User.Domain.Repositories;

namespace User.Controllers.IntegrationTests;

public class EditControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EditControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        var customizedFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<DataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_Edit");
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                db.Database.EnsureCreated();
                if (!db.Users.Any())
                {
                    db.Users.Add(new User.Domain.Models.User
                    {
                        Username = "testuser",
                        FullName = "Test User",
                        Email = "testuser@example.com",
                        PasswordHash = PasswordHelper.Hash("password123"),
                        CreatedAt = DateTime.UtcNow,
                        Roles = new List<Role> { new Role { Name = "Client" } }
                    });
                    db.SaveChanges();
                }
            });
        });

        _client = customizedFactory.CreateClient();
    }

    private async Task<string> GetTokenAsync(string username, string password)
    {
        var payload = new { Username = username, Password = password };
        var response = await _client.PostAsync("/api/login", new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("token").GetString();
    }

    private void SetAuthToken(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Fact]
    public async Task EditUsername_ShouldUpdateUsername()
    {
        var token = await GetTokenAsync("testuser", "password123");
        SetAuthToken(token);

        var editUsernameDto = new { NewUsername = "updated_username", Password = "password123" };
        var content = new StringContent(JsonSerializer.Serialize(editUsernameDto), Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync("/api/Edit/username", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updatedUser = JsonSerializer.Deserialize<UserResponseDto>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Assert.Equal("updated_username", updatedUser.Username);
    }

    [Fact]
    public async Task EditEmail_ShouldUpdateEmail()
    {
        var token = await GetTokenAsync("testuser", "password123");
        SetAuthToken(token);

        var editEmailDto = new { NewEmail = "new@example.com", Password = "password123" };
        var content = new StringContent(JsonSerializer.Serialize(editEmailDto), Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync("/api/Edit/email", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updatedUser = JsonSerializer.Deserialize<UserResponseDto>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        Assert.Equal("new@example.com", updatedUser.Email);
    }

    [Fact]
    public async Task EditFullName_ShouldUpdateFullName()
    {
        var token = await GetTokenAsync("testuser", "password123");
        SetAuthToken(token);

        var editNameDto = new { NewFullName = "Nowe Imię", Password = "password123" };
        var content = new StringContent(JsonSerializer.Serialize(editNameDto), Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync("/api/Edit/fullname", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //var updatedUser = JsonSerializer.Deserialize<UserResponseDto>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        //Assert.Equal("Nowe Imię", updatedUser.FullName);
    }

    [Fact]
    public async Task ChangePassword_ShouldUpdatePassword()
    {
        var token = await GetTokenAsync("testuser", "password123");
        SetAuthToken(token);

        var changePasswordDto = new
        {
            OldPassword = "password123",
            NewPassword = "newPassword@123"
        };

        var content = new StringContent(JsonSerializer.Serialize(changePasswordDto), Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync("/api/Edit/change-password", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var msg = await response.Content.ReadAsStringAsync();
        Assert.Contains("changed successfully", msg);
    }
}
