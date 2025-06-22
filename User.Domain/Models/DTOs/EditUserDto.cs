using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models.DTOs;

public class EditUserDto
{
    public string FullName { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }
}
