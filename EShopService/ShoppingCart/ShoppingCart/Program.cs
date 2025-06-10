using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Infrastructure.Repositories;
using ShoppingCart.Domain.Seeders;
using ShoppingCart.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Design;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        // Baza danych
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(connectionString), ServiceLifetime.Transient);

        // Repozytorium
        builder.Services.AddScoped<ICartRepository, InMemoryCartRepository>();

        // Pamiêæ podrêczna
        builder.Services.AddMemoryCache();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CartService).Assembly));

        // Register dependencies (DIP)
        builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();
        builder.Services.AddSingleton<ICartAdder, CartService>();
        builder.Services.AddSingleton<ICartRemover, CartService>();
        builder.Services.AddSingleton<ICartReader, CartService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Dane pocz¹tkowe
        builder.Services.AddScoped<IShoppingCartSeeder, ShoppingCartSeeder>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            await db.Database.MigrateAsync();
            var seeder = scope.ServiceProvider.GetRequiredService<IShoppingCartSeeder>();
            await seeder.Seed();
        }

        app.Run();
    }
}
