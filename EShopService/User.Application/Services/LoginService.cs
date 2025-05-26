using System.Security.Claims;
using User.Domain.Exceptions.Login;
using User.Domain.Models;
using User.Domain;
using User.Domain.Repositories;

namespace User.Application.Services;

public class LoginService : ILoginService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly Queue<int> _userLoggedIdsQueue;
    private readonly IRepository _userRepository;

    public LoginService(
        IJwtTokenService jwtTokenService,
        Queue<int> userLoggedInsQueue,
        IRepository userRepository)
    {
        _jwtTokenService = jwtTokenService;
        _userLoggedIdsQueue = userLoggedInsQueue;
        _userRepository = userRepository;
    }

    public string Login(string username, string password)
    {
        // 1. Keep admin credentials logic for testing
        if (username == "admin" && password == "password")
        {
            var roles = new List<string> { "Client", "Employee", "Administrator" };
            var token = _jwtTokenService.GenerateToken(123, roles);
            _userLoggedIdsQueue.Enqueue(123);
            return token;
        }

        var userTask = _userRepository.GetUserByUsernameAsync(username);
        userTask.Wait();
        var user = userTask.Result;

        if (user == null)
            throw new InvalidCredentialsException();

        if (!PasswordHelper.Verify(password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var userRoles = user.Roles?.Select(r => r.Name).ToList() ?? new List<string>();
        var userToken = _jwtTokenService.GenerateToken(user.Id, userRoles);
        _userLoggedIdsQueue.Enqueue(user.Id);
        return userToken;
    }
}
