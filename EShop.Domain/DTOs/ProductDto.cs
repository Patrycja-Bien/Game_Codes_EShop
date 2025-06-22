using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogue.Domain.DTOs;

public class ProductDto
{
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable;
}
