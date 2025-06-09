using Microsoft.AspNetCore.Mvc;
using EShop.Application.Services;
using EShop.Domain.Models;
using Orders.Application.Services;
using Orders.Domain.Models;
using Microsoft.AspNetCore.Authorization;

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
    [AllowAnonymous]
    public ActionResult<ShoppingCart> GetCart(int userId)
        => Ok(_cartService.GetCartAsync(userId));

    [HttpPost("add")]
    [AllowAnonymous]
    public IActionResult AddItem(int userId, int productId, int quantity)
    {
        _cartService.AddItemAsync(userId, productId, quantity);
        return Ok();
    }

    [HttpPost("remove")]
    [AllowAnonymous]
    public IActionResult RemoveItem(int userId, int productId)
    {
        _cartService.RemoveItemAsync(userId, productId);
        return Ok();
    }

    [HttpPost("clear")]
    [AllowAnonymous]
    public IActionResult ClearCart(int userId)
    {
        _cartService.ClearCartAsync(userId);
        return Ok();
    }

    [HttpPost("checkout")]
    [AllowAnonymous]
    public async Task<IActionResult> Checkout(int userId, [FromServices] IOrdersService ordersService)
    {
        var cart = await _cartService.GetCartAsync(userId);
        if (cart == null || cart.Items.Count == 0)
            return BadRequest("Cart is empty.");

        var order = new Order
        {
            CartItems = cart.Items.Select(i => new CartItem
            {
                ProductId = i.ProductId,
                Name = i.Name,
                Price = i.Price,
                Sku = i.Sku,
                Quantity = i.Quantity

            }).ToList(),
            TotalAmount = cart.Items.Sum(i => i.Price * i.Quantity),
        };

        var createdOrder = await ordersService.AddAsync(order);

        await _cartService.ClearCartAsync(userId);

        return Ok(createdOrder);
    }

}
