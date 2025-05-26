using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Response;

namespace User.Application.Services;

public interface IUserService
{
    public Task<UserResponseDto?> GetUserDataAsync(int userId);
    public Task<User.Domain.Models.User> AddUserAsync(User.Domain.Models.User user);
}
