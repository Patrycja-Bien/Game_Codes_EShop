using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using User.Application.Services;
using User.Domain.Models.JWT;
using User.Domain.Repositories;
using User.Domain.Models.Profiles;
using User.Domain.Seeders;
using User.Application.Producer;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace UserService;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Baza danych
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        builder.Services.AddDbContext<DataContext>(options =>
             options.UseSqlServer(connectionString), ServiceLifetime.Transient);

        //if (!builder.Environment.IsEnvironment("Testing"))
        //{
        //    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        //ConnectionMultiplexer.Connect("redis:6379"));
        //}

        //Memory Cache
        builder.Services.AddMemoryCache();

        //Repozytorium
        builder.Services.AddScoped<IRepository, Repository>();

        // JWT config - token
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        builder.Services.Configure<JwtSettings>(jwtSettings);

        //Mapper
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        //Kolejka
        builder.Services.AddSingleton<Queue<int>>();


        //Autentykacja
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var rsa = RSA.Create();
            try
            {
                rsa.ImportFromPem(File.ReadAllText("../src/public.key")); // Za³aduj klucz publiczny RSA
            }
            catch (Exception ex) // na potrzeby testów
            {
                rsa.ImportFromPem(File.ReadAllText("../../../../../public.key"));
            }
            var publicKey = new RsaSecurityKey(rsa);

            var jwtConfig = jwtSettings.Get<JwtSettings>();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = publicKey
            };
        });

        //Autoryzacja
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Administrator"));
        });

        //Serwisy
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IUserService, User.Application.Services.UserService>();
        builder.Services.AddScoped<IEditUserService, EditUserService>();
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
        builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();


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
        builder.Services.AddScoped<IUsersSeeder, UsersSeeder>();

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

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            if (db.Database.IsRelational())
            {
                db.Database.Migrate();
            }
            var seeder = scope.ServiceProvider.GetRequiredService<IUsersSeeder>();
            await seeder.Seed();
        }

        app.MapControllers();

        app.Run();
    }
}