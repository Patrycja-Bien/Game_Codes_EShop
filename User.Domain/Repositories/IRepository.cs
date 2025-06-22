using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using User.Domain.Models;

namespace User.Domain.Repositories;

public interface IRepository
{
    #region User
    Task<User.Domain.Models.User> GetUserAsync(int id);
    Task<User.Domain.Models.User> AddUserAsync(User.Domain.Models.User user);
    Task<User.Domain.Models.User> UpdateUserAsync(User.Domain.Models.User user);
    Task<List<User.Domain.Models.User>> GetAllUsersAsync();
    Task<User.Domain.Models.User> GetUserByUsernameAsync(string username);
    #endregion

}
