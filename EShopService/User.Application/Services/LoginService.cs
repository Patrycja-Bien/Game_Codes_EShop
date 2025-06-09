using System.Security.Claims;
using User.Domain.Exceptions.Login;
using User.Domain;
using User.Domain.Repositories;
using User.Application.Producer;
using User.Domain.Helpers;

namespace User.Application.Services;

public class LoginService : ILoginService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly Queue<int> _userLoggedIdsQueue;
    private readonly IRepository _userRepository;
    private readonly IKafkaProducer _kafkaProducer;

    public LoginService(
        IJwtTokenService jwtTokenService,
        Queue<int> userLoggedInsQueue,
        IRepository userRepository,
        IKafkaProducer kafkaProducer)
    {
        _jwtTokenService = jwtTokenService;
        _userLoggedIdsQueue = userLoggedInsQueue;
        _userRepository = userRepository;
        _kafkaProducer = kafkaProducer;
    }

    public string Login(string username, string password)
    {   
        var userTask = _userRepository.GetUserByUsernameAsync(username);
        userTask.Wait();
        var user = userTask.Result;

        if (user == null)
            throw new InvalidCredentialsException();

        if (!PasswordHelper.Verify(password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var roles = user.Roles?.Select(r => r.Name).ToList() ?? new List<string> { "Client" };
        var token = _jwtTokenService.GenerateToken(user.Id, roles);
        _userLoggedIdsQueue.Enqueue(user.Id);
       
        _kafkaProducer.SendMessageAsync("after-login-email-topic", user.Email);

        return token;
    }
}
