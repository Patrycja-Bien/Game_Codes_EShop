using User.Domain.Exceptions.Login;
using User.Domain.Models;


namespace User.Application.Services;

public class LoginService : ILoginService
{
    protected IJwtTokenService _jwtTokenService;
    //private readonly IMessageQueue _messageQueue;

    public LoginService(IJwtTokenService jwtTokenService) //IMessageQueue messageQueue)
    {
        _jwtTokenService = jwtTokenService;
        //_messageQueue = messageQueue;
    }

    public string Login(string username, string password)
    {
        if (username == "admin" && password == "password")
        {
            var roles = new List<string> { "Client", "Employee", "Administrator" };
            var token = _jwtTokenService.GenerateToken(123, roles);
            return token;
        }
        else
        {
            throw new InvalidCredentialsException();
        }

    }
}
