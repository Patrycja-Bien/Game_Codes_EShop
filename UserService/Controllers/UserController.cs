using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Application.Services;
using User.Domain.Models.Response;
using User.Domain.Models.DTOs;
using User.Domain.Helpers;

namespace UserService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly User.Application.Services.IUserService _userService;

    public UserController(User.Application.Services.IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponseDto>> GetUserData()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var data = await _userService.GetUserDataAsync(int.Parse(userId));
        if (data == null)
            return NotFound("User not found");

        return Ok(data);
    }

    [HttpGet("Get-All")]
    [AllowAnonymous]
    public async Task<ActionResult<List<UserResponseDto?>>> GetAllUsersData()
    {
        var dtos = await _userService.GetAllUsersDataAsync();
        return dtos;
    }

    [HttpPost("Create-User")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        var user = new User.Domain.Models.User
        {
            Username = dto.Username,
            FullName = dto.Fullname,
            Email = dto.Email,
            PasswordHash = PasswordHelper.Hash(dto.Password),
            CreatedAt = DateTime.UtcNow
        };
        var result = await _userService.AddUserAsync(user);
        return Ok(ToResponse(result));
    }

    private UserResponseDto ToResponse(User.Domain.Models.User u) => new()
    {
        Id = u.Id,
        Username = u.Username,
        Email = u.Email,
        CreatedAt = u.CreatedAt,
        LastLoginAt = u.LastLoginAt ?? DateTime.MinValue,
        Roles = u.Roles?.Select(r => r.Name).ToList() ?? new()
    };
}



