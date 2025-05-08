using Game_Codes_EShop_Domain.Models;
using Game_Codes_EShop_Domain.Repositories;
using Game_Codes_EShopService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Game_Codes_EShopService_Intergration_Tests;

public class GameController_Integration_Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public GameController_Integration_Tests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // pobranie dotychczasowej konfiguracji bazy danych
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                    //// usuniêcie dotychczasowej konfiguracji bazy danych
                    services.Remove(dbContextOptions);

                    // Stworzenie nowej bazy danych
                    services
                        .AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MyDBForTest"));

                });
            });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsAllGames_ExpectedFourGames()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            // Pobranie kontekstu bazy danych
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Games.RemoveRange(dbContext.Games);

            // Stworzenie obiektu
            dbContext.Games.AddRange(
                new Game { Game_Name = "TestGame1" },
                new Game { Game_Name = "TestGame2" },
                new Game { Game_Name = "TestGame3" },
                new Game { Game_Name = "TestGame4" }
            );
            // Zapisanie obiektu
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Game>>();
        Assert.Equal(4, products?.Count);
    }

    [Fact]
    public async Task Post_AddThousandsGames_ExceptedThousandsGames()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Games.RemoveRange(dbContext.Games);
            dbContext.SaveChanges();
            var tasks = new List<Task>();

            for (int i = 0; i < 10000; i++)
            {
                int index = 1;
                tasks.Add(Task.Run(async () =>
                {
                    using (var scope = _factory.Services.CreateScope())
                    {
                        // Pobranie kontekstu bazy danych
                        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                        {
                            dbContext.Games.Add(new Game { Game_Name = "TestGame" + index });
                            dbContext.SaveChanges();
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks);

        }

        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        response.EnsureSuccessStatusCode();
        var games = await response.Content.ReadFromJsonAsync<List<Game>>();
        Assert.Equal(10000, games?.Count);
    }

    [Fact]
    public async Task Post_AddThousandsGamesAsync_ExceptedThousandsGames()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            // Pobranie kontekstu bazy danych
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Games.RemoveRange(dbContext.Games);
            await dbContext.SaveChangesAsync();

        }

        var tasks = new List<Task>();

        for (int i = 0; i < 10000; i++)
        {
            int index = i;
            tasks.Add(Task.Run(async () =>
            {
                using (var scope = _factory.Services.CreateScope())
                {
                    // Pobranie kontekstu bazy danych
                    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    {
                        dbContext.Games.Add(new Game { Game_Name = "TestGame" + index });
                        await dbContext.SaveChangesAsync();
                    }
                }
            }));
        }
        await Task.WhenAll(tasks);


        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Game>>();
        Assert.Equal(10000, products?.Count);
    }

    [Fact]
    public async Task Add_AddProduct_ExceptedOneProduct()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            // Pobranie kontekstu bazy danych
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Games.RemoveRange(dbContext.Games);
            dbContext.SaveChanges();

            // Act
            var category = new Category
            {
                Category_Name = "TestCategoryName"
            };

            var game = new Game
            {
                Game_Name = "Game",
                Game_Category = category
            };

            var json = JsonConvert.SerializeObject(game);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync("/api/Product", content);

            var result = await dbContext.Games.ToListAsync();

            // Assert
            Assert.Equal(1, result?.Count);
        }
    }
}