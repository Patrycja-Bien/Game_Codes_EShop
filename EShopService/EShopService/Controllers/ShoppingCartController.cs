using Microsoft.AspNetCore.Mvc;
using EShop.Application.Services;
using EShop.Domain.Models;
using Orders.Application.Services;
using Orders.Domain.Models;

namespace EShopService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartService _cartService;

    public ShoppingCartController(IShoppingCartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public ActionResult<ShoppingCart> GetCart(int userId)
        => Ok(_cartService.GetCartAsync(userId));

    [HttpPost("add")]
    public IActionResult AddItem(int userId, int productId, int quantity)
    {
        _cartService.AddItemAsync(userId, productId, quantity);
        return Ok();
    }

    [HttpPost("remove")]
    public IActionResult RemoveItem(int userId, int productId)
    {
        _cartService.RemoveItemAsync(userId, productId);
        return Ok();
    }

    [HttpPost("clear")]
    public IActionResult ClearCart(int userId)
    {
        _cartService.ClearCartAsync(userId);
        return Ok();
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(int userId, [FromServices] IOrdersService ordersService)
    {
        var cart = await _cartService.GetCartAsync(userId);
        if (cart == null || cart.Items.Count == 0)
            return BadRequest("Cart is empty.");

        var order = new Order
        {
            Products = cart.Items.Select(i => new Product
            {
                Id = i.ProductId,
                Name = i.Name,
                Price = i.Price,
                Sku = i.Sku,
            }).ToList(),
            TotalAmount = cart.Items.Sum(i => i.Price * i.Quantity),
        };

        var createdOrder = await ordersService.AddAsync(order);

        await _cartService.ClearCartAsync(userId);

        return Ok(createdOrder);
    }

}
