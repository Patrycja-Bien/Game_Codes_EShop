using EShop.Application.Services;
using EShop.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Domain.DTOs;

namespace EShopService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/<ProductController>
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await _productService.GetAllAsync();
        return Ok(result);
    }

    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _productService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST api/<ProductController>
    [HttpPost]
    [Authorize]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<ActionResult> Post([FromBody] Product product)
    {
        var result = await _productService.AddAsync(product);

        return Ok(result);
    }

    // PUT api/<ProductController>/5
    [HttpPut("{id}")]
    [Authorize]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<ActionResult> Put(int id, [FromBody] Product product)
    {
        var result = await _productService.UpdateAsync(product);

        return Ok(result);
    }

    // DELETE api/<ProductController>/5
    [HttpDelete("{id}")]
    [Authorize]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await _productService.GetAsync(id);
        product.Deleted = true;
        var result = await _productService.UpdateAsync(product);

        return Ok(result);
    }

    [HttpPatch]
    [Authorize]
    [Authorize(Policy = "EmployeeOnly")]
    public ActionResult Add([FromBody] Product product)
    {
        var result = _productService.Add(product);

        return Ok(result);
    }

    [HttpGet("Is-Available/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductDto>> IsAvailable(int id, [FromQuery] int quantity)
    {
        var result = await _productService.IsProductValidToProcessAsync(id, quantity);
        return Ok(result);
    }
}