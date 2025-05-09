using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Application.Services;
using User_Domain.Exceptions;
using User_Domain.Requests;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    protected ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }


    [HttpPost]
    public IActionResult Login([FromBody] User_Domain.Requests.LoginRequest request)
    {
        try
        {
            var token = _loginService.Login(request.Username, request.Password);
            return Ok(new { token });
        }
        catch (InvalidCredentialsException)
        {
            return Unauthorized();
        }
    }

    [HttpGet]
    [Authorize]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AdminPage()
    {
        return Ok("Hello Administrator");
    }
}
