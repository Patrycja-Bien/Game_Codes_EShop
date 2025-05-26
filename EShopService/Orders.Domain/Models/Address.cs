using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Models;

public class Address
{
    [Key]
    public int AddressId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }

    public string Phone { get; set; }
    public string Email { get; set; }
}
