using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models;

public class UserLoggedInMessage
{
    public string Username { get; set; }
    public DateTime LoginTime { get; set; }
}

