using ProductCatalogue.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Processing.Interfaces;

public interface IExternalProductsService
{
    Task<ProductDto> GetProductByIdAsync(int productId);
}
