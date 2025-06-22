using MediatR;
using ShoppingCart.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Commands;

public class ProcessCartToOrderCommand : IRequest<OrderDto>
{
    public int CartId { get; set; }
    public string Email { get; set; }
}
