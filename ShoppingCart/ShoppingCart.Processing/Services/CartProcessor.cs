using EShop.Application.Services;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Processing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Processing.Services;

public class CartProcessor : ICartProcessor
{
    private readonly ICartReader _cartReader;
    private readonly IProductService _productService;

    public CartProcessor(ICartReader cartReader, IProductService productService)
    {
        _cartReader = cartReader;
        _productService = productService;
    }

    public async Task ProcessCartAsync(int cartId)
    {
        var cart = _cartReader.GetCart(cartId);
        if (cart == null) return;

        foreach (var product in cart.Items)
        {
            var productDto = await _productService.GetAsync(product.Id);
            Console.WriteLine($"Processing: {productDto.Name} - {productDto.Price} zł");
        }
    }
}

