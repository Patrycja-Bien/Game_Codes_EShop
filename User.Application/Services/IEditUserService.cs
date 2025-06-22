using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Services;

public interface IEditUserService
{
    public Task<User.Domain.Models.User> EditUsernameAsync(User.Domain.Models.User user);
    public Task<User.Domain.Models.User> EditFullNameAsync(User.Domain.Models.User user);
    public Task<User.Domain.Models.User> EditEmailAsync(User.Domain.Models.User user);
    public Task<User.Domain.Models.User> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
}
