using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Domain.Models;
using User.Application.Services;
using User.Domain.Models.Response;
using User.Domain.Models.DTOs;

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
    [Authorize]
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

    [HttpGet("GetAll")]
    [Authorize]
    public async Task<ActionResult<List<UserResponseDto?>>> GetAllUsersData()
    {
        var dtos = await _userService.GetAllUsersDataAsync();
        return dtos;
    }

    [HttpPost("CreateUser")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var user = new User.Domain.Models.User
        {
            Username = dto.UserName,
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = PasswordHelper.Hash(dto.Password),
            CreatedAt = DateTime.UtcNow
        };
        var result = await _userService.AddUserAsync(user);
        return Ok(result);
    }
}

