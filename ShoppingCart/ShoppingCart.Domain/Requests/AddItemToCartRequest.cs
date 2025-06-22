using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Requests;

public class AddItemToCartRequest
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
