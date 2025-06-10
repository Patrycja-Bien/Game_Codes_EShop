using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Domain.Models;

public class Cart
{
    [Key]
    public int Id { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public decimal TotalPrice => Items.Sum(p => p.Price * p.Quantity);
}
