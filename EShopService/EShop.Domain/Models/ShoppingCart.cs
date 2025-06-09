using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Models;

public class ShoppingCart
{
    [Key]
    public int UserId { get; set; }
    public List<CartItem> Items { get; set; } = new();
}
