using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Enums;

public enum OrderStatus
{
    New,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
