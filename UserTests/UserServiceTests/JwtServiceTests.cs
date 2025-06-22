using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Application.Services;
using User.Domain.Models.JWT;

namespace User.Application.Tests;

public class JwtServiceTests
{
    private readonly JwtTokenService _jwtTokenService;

    public JwtServiceTests()
    {
        var jwtSettings = new JwtSettings
        {
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiresInMinutes = 60
        };

        var mockOptions = new Mock<IOptions<JwtSettings>>();
        mockOptions.Setup(x => x.Value).Returns(jwtSettings);

        _jwtTokenService = new JwtTokenService(mockOptions.Object);
    }

    [Fact]
    public void GenerateToken_Returns_ValidJwtToken()
    {
        // Arrange
        var userId = 123;
        var roles = new List<string> { "Administrator", "User" };

        // Act
        var tokenString = _jwtTokenService.GenerateToken(userId, roles);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        Assert.True(handler.CanReadToken(tokenString));

        var token = handler.ReadJwtToken(tokenString);
        Assert.Equal("TestIssuer", token.Issuer);
        Assert.Equal("TestAudience", token.Audiences.Single());

        var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.NotNull(userIdClaim);
        Assert.Equal(userId.ToString(), userIdClaim.Value);

        var roleClaims = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        Assert.Contains("Administrator", roleClaims);
        Assert.Contains("User", roleClaims);

        Assert.True(token.ValidTo > DateTime.UtcNow);
    }
}
