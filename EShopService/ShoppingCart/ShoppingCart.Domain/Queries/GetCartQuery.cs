using MediatR;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Queries;

public class GetCartQuery : IRequest<Cart>
{
    public int CartId { get; set; }
}
