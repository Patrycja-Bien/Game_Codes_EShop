using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Queries;
using ShoppingCart.Domain.Requests;
using System.Net.Http;
using System.Text.Json;

namespace ShoppingCart.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly HttpClient _httpClient;

    public CartController(IMediator mediator, HttpClient httpClient)
    {
        _mediator = mediator;
        _httpClient = httpClient;
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductToCart([FromBody] AddItemToCartCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    //[HttpPost("add-product")]
    //public async Task<IActionResult> AddProductToCart([FromBody] AddItemToCartRequest request)
    //{
    //    var response = _httpClient.GetAsync($"http://eshopservice:8080/api/product/Is-Available/{request.ProductId}?quantity={request.Quantity}").Result;

    //    if (!response.IsSuccessStatusCode)
    //        return BadRequest("Failed to get product information.");

    //    var productJson = await response.Content.ReadAsStringAsync();
    //    var product = JsonSerializer.Deserialize<Item>(productJson, new JsonSerializerOptions
    //    {
    //        PropertyNameCaseInsensitive = true
    //    });

    //    if (product == null || !product.IsAvailable)
    //        return BadRequest("Product not available.");

    //    var command = new AddItemToCartCommand
    //    {
    //        CartId = request.CartId,
    //        ItemId = request.ProductId,
    //        ItemName = product.Name,
    //        Quantity = product.Quantity,
    //        Price = product.Price
    //    };

    //    await _mediator.Send(command);
    //    return Ok();
    //}


    [HttpPost("remove-product")]
    public async Task<IActionResult> RemoveItemFromCart([FromBody] RemoveItemFromCartCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("get-cart-by-id-{cartId}")]
    public async Task<IActionResult> GetCart(int cartId)
    {
        var query = new GetCartQuery { CartId = cartId };
        var result = await _mediator.Send(query);
        return result == null ? NotFound("Cart not found") : Ok(result);
    }

    [HttpGet("get-all-carts")]
    public async Task<IActionResult> GetAllCarts()
    {
        var query = new GetAllCartsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("process-cart-to-order")]
    public async Task<IActionResult> ProcessCartToOrder([FromBody] ProcessCartToOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return result == null ? NotFound("Cart not found") : Ok(result);
    }

}
