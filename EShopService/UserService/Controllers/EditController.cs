using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Domain.Helpers;
using User.Domain.Models.DTOs;
using User.Domain.Models.Response;

namespace UserService.Controllers;

public class EditController : ControllerBase
{
    private readonly User.Application.Services.IEditUserService _editUserService;
    private readonly User.Application.Services.IUserService _userService;

    public EditController(User.Application.Services.IEditUserService editUserService, User.Application.Services.IUserService userService)
    {
        _editUserService = editUserService;
        _userService = userService;
    }

    [HttpPatch("Edit User Data: Username")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> EditUserData(string newUsername, string password)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var dto = await _userService.GetUserDataAsync(Int32.Parse(userId));
        if (dto == null)
            return NotFound();

        var user = new User.Domain.Models.User
        {
            Id = dto.Id,
            Username = newUsername,
            Email = dto.Email,
            PasswordHash = PasswordHelper.Hash(password),
        };

        var updatedUser = await _editUserService.EditUsernameAsync(user);

        var result = new UserResponseDto
        {
            Id = updatedUser.Id,
            Username = updatedUser.Username,
            Email = updatedUser.Email,
            CreatedAt = updatedUser.CreatedAt,
            LastLoginAt = updatedUser.LastLoginAt ?? DateTime.MinValue,
            Roles = updatedUser.Roles?.Select(r => r.Name).ToList() ?? new List<string>()
        };

        return Ok(result);
    }

    [HttpPatch("Edit User Data: Full Name")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> EditUserFullName(string newFullName, string password)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        var dto = await _userService.GetUserDataAsync(Int32.Parse(userId));
        if (dto == null)
            return NotFound();
        var user = new User.Domain.Models.User
        {
            Id = dto.Id,
            FullName = newFullName,
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = PasswordHelper.Hash(password)
        };
        var updatedUser = await _editUserService.EditFullNameAsync(user);
        var result = new UserResponseDto
        {
            Id = updatedUser.Id,
            Username = updatedUser.Username,
            Email = updatedUser.Email,
            CreatedAt = updatedUser.CreatedAt,
            LastLoginAt = updatedUser.LastLoginAt ?? DateTime.MinValue,
            Roles = updatedUser.Roles?.Select(r => r.Name).ToList() ?? new List<string>()
        };
        return Ok(result);
    }

    [HttpPatch("Edit User Data: Email")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> EditUserEmail(string newEmail, string password)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        var dto = await _userService.GetUserDataAsync(Int32.Parse(userId));
        if (dto == null)
            return NotFound();
        var user = new User.Domain.Models.User
        {
            Id = dto.Id,
            Username = dto.Username,
            Email = newEmail,
            PasswordHash = PasswordHelper.Hash(password)
        };
        var updatedUser = await _editUserService.EditEmailAsync(user);
        var result = new UserResponseDto
        {
            Id = updatedUser.Id,
            Username = updatedUser.Username,
            Email = updatedUser.Email,
            CreatedAt = updatedUser.CreatedAt,
            LastLoginAt = updatedUser.LastLoginAt ?? DateTime.MinValue,
            Roles = updatedUser.Roles?.Select(r => r.Name).ToList() ?? new List<string>()
        };
        return Ok(result);
    }

    [HttpPatch("Change password")]
    [Authorize]
    public async Task<IActionResult> ResetPassword(string OldPassword, string NewPassword)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _editUserService.ChangePasswordAsync(Int32.Parse(userId), OldPassword, NewPassword);
        if (result == null)
            return NotFound($"For user with Id {userId} password change failed.");
        else
            return Ok($"Password for user with Id {userId} changed successfully.");
    }

}
