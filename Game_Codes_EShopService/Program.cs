using Game_Codes_EShop_Domain.Models;
using Game_Codes_EShop_Domain.Repositories;
using Game_Codes_EShop_Domain.Seeders;
using Microsoft.EntityFrameworkCore;
using Game_Codes_EShop_Application.Services;


namespace Game_Codes_EShopService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"), ServiceLifetime.Transient);
        builder.Services.AddScoped<IGame_Repository, Game_Repository>();


        // Add services to the container.
        builder.Services.AddScoped<ICredit_Card_Service, Credit_Card_Service>();
        builder.Services.AddScoped<IGame_Service, Game_Service>();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMemoryCache();



        builder.Services.AddScoped<IEShopSeeder, EShopSeeder>();
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


        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IEShopSeeder>();
        await seeder.Seed();

        app.Run();
    }
}