using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Domain.Models;
using User.Application.Services;

namespace UserService.Controllers;

//[Authorize]
//[ApiController]
//[Route("api/user")]
//public class UsersController : ControllerBase
//{
//    private readonly UsersService _userService;

//    public UsersController(UsersService userService)
//    {
//        _userService = userService;
//    }

//    [HttpGet("me")]
//    public async Task<IActionResult> GetUserData()
//    {
//        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        if (userId == null)
//            return Unauthorized();

//        var data = await _userService.GetUserDataAsync(int.Parse(userId));
//        if (data == null)
//            return NotFound();

//        return Ok(data);
//    }
//}

