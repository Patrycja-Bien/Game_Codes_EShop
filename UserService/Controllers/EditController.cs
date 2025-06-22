using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Application.Services;
using User.Domain.Helpers;
using User.Domain.Models.DTOs;
using User.Domain.Models.Response;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EditController : ControllerBase
{
    private readonly IEditUserService _editUserService;
    private readonly IUserService _userService;

    public EditController(IEditUserService editUserService, IUserService userService)
    {
        _editUserService = editUserService;
        _userService = userService;
    }

    [HttpPatch("username")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> EditUsername([FromBody] EditUsernameRequestDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var user = await _userService.GetFullUserAsync(int.Parse(userId));
        if (user == null) return NotFound();

        if (!PasswordHelper.Verify(request.Password, user.PasswordHash))
            return BadRequest("Invalid password.");

        user.Username = request.NewUsername;
        var updatedUser = await _editUserService.EditUsernameAsync(user);

        return Ok(ToResponse(updatedUser));
    }

    [HttpPatch("fullname")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> EditFullName([FromBody] EditFullNameRequestDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var user = await _userService.GetFullUserAsync(int.Parse(userId));
        if (user == null) return NotFound();

        if (!PasswordHelper.Verify(request.Password, user.PasswordHash))
            return BadRequest("Invalid password.");

        user.FullName = request.NewFullName;
        var updatedUser = await _editUserService.EditFullNameAsync(user);

        return Ok(ToResponse(updatedUser));
    }

    [HttpPatch("email")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> EditEmail([FromBody] EditEmailRequestDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var user = await _userService.GetFullUserAsync(int.Parse(userId));
        if (user == null) return NotFound();

        if (!PasswordHelper.Verify(request.Password, user.PasswordHash))
            return BadRequest("Invalid password.");

        user.Email = request.NewEmail;
        var updatedUser = await _editUserService.EditEmailAsync(user);

        return Ok(ToResponse(updatedUser));
    }

    [HttpPatch("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        try
        {
            var updated = await _editUserService.ChangePasswordAsync(int.Parse(userId), request.OldPassword, request.NewPassword);
            return Ok($"Password for user {updated.Id} changed successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
