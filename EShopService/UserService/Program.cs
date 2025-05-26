using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;
using User.Application.Services;
using User.Domain.Models.JWT;
using User.Domain.Repositories;
using User.Domain.Models;
using User.Domain.Models.Requests;
using User.Domain.Models.Profiles;
using User.Domain.Repositories;
using AutoMapper;

namespace UserService;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Baza danych
        builder.Services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"), ServiceLifetime.Transient);

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
            rsa.ImportFromPem(File.ReadAllText("../src/public.key"));// Za³aduj klucz publiczny RSA
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
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


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

        app.Run();
    }
}