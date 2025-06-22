using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Orders.Domain.Repositories;
using StackExchange.Redis;
using Orders.Application.Services;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Orders.Domain.Seeders;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Baza danych
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        builder.Services.AddDbContext<EShop.Domain.Repositories.DataContext>(options =>
            options.UseSqlServer(connectionString), ServiceLifetime.Transient);        
        builder.Services.AddDbContext<Orders.Domain.Repositories.DataContext>(options =>
            options.UseSqlServer(connectionString), ServiceLifetime.Transient);

        //Repozytorium
        builder.Services.AddScoped<IRepository, Repository>();

        //Pamiêæ podrêczna
        builder.Services.AddMemoryCache();

        ////Redis
        //builder.Services.AddStackExchangeRedisCache(options =>
        //{
        //    options.Configuration = "redis:6379"; // Redis port
        //    options.InstanceName = "Game_Codes_EShop_Redis";
        //});

        //Autentykacja
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText("../src/public.key")); //RSA
            var publicKey = new RsaSecurityKey(rsa);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "EShopNetCourse",
                ValidAudience = "Eshop",
                IssuerSigningKey = publicKey
            };
        });

        //Autoryzacja
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Administrator"));
            options.AddPolicy("EmployeeOnly", policy =>
                policy.RequireRole("Employee"));
        });

        //Serwisy
        builder.Services.AddScoped<IOrdersService, Orders.Application.Services.OrdersService>();

        //Kontrolery
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Wpisz token w formacie: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
        });

        //Dane pocz¹tkowe
        builder.Services.AddScoped<IOrdersSeeder, OrdersSeeder>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();


        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            await db.Database.EnsureCreatedAsync();
            var seeder = scope.ServiceProvider.GetRequiredService<IOrdersSeeder>();
            await seeder.Seed();
        }


        app.Run();
    }
}
