using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.DTOs;

public class OrderDto
{
    public int CartId { get; set; }
    public string Email { get; set; } = "example@gmail.com";
    public List<Item> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
}