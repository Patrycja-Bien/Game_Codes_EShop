using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Commands;

public class AddItemToCartCommand : IRequest
{
    public int CartId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
