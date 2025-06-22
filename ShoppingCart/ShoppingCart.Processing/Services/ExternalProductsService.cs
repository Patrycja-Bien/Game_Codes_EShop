using Microsoft.Extensions.Logging;
using ProductCatalogue.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Processing.Interfaces;
using EShop.Application.Services;

namespace ShoppingCart.Processing.Services;

public class ExternalProductsService : IExternalProductsService
{
    private readonly IProductService _productService;

    public ExternalProductsService(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDto> GetProductByIdAsync(int productId)
    {
        var product = await _productService.GetAsync(productId);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
