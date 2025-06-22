using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models.DTOs;

public class EditEmailRequestDto
{
    public string NewEmail { get; set; } = default!;
    public string Password { get; set; } = default!;
}
