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
    Task<User.Domain.Models.UserRequest> GetUserAsync(int id);
    Task<User.Domain.Models.UserRequest> AddUserAsync(User.Domain.Models.UserRequest user);
    Task<User.Domain.Models.UserRequest> UpdateUserAsync(User.Domain.Models.UserRequest user);
    Task<List<User.Domain.Models.UserRequest>> GetAllUsersAsync();
    #endregion

}
