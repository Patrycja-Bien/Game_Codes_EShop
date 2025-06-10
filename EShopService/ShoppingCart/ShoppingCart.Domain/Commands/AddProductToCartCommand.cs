using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Commands;

public class AddProductToCartCommand : IRequest
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
}
