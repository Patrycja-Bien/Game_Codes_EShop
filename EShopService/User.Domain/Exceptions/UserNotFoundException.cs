using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions;

public class UserNotFoundException : Exception
{

    public UserNotFoundException() : base("User not found exception") { }

    public UserNotFoundException(Exception innerException) : base("User not found", innerException) { }
}
