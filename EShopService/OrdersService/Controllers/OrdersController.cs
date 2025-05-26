using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Services;
using Orders.Domain.Models;

namespace OrdersService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private IOrdersService _ordersService;
    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    // GET: api/<OrdersController>
    [HttpGet]
    [Authorize]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<ActionResult> Get()
    {
        var result = await _ordersService.GetAllAsync();
        return Ok(result);
    }

    // GET api/<OrdersController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _ordersService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST api/<OrdersController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Order order)
    {
        var result = await _ordersService.AddAsync(order);

        return Ok(result);
    }

    // PUT api/<OrdersController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] Order order)
    {
        var result = await _ordersService.UpdateAsync(order);

        return Ok(result);
    }

    // DELETE api/<OrdersController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var order = await _ordersService.GetAsync(id);
        order.Deleted = true;
        var result = await _ordersService.UpdateAsync(order);

        return Ok(result);
    }

    [HttpPatch]
    public ActionResult Add([FromBody] Order order)
    {
        var result = _ordersService.Add(order);

        return Ok(result);
    }
}
