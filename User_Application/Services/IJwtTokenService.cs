namespace User_Application.Services;

public interface IJwtTokenService
{
    public string GenerateToken(int userId, List<string> roles);

}