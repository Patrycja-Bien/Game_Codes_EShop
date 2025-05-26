using EShop.Application.Services;
using EShop.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopService.Controllers;

public class CategoryController : ControllerBase
{
    private ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/<CategoryController>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await _categoryService.GetAllAsync();
        return Ok(result);
    }

    // GET api/<CategoryController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _categoryService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST api/<CategoryController>
    [HttpPost]
    [Authorize]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<ActionResult> Post([FromBody] Category category)
    {
        var result = await _categoryService.AddAsync(category);

        return Ok(result);
    }

    // PUT api/<CategoryController>/5
    [HttpPut("{id}")]
    [Authorize]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<ActionResult> Put(int id, [FromBody] Category category)
    {
        var result = await _categoryService.UpdateAsync(category);

        return Ok(result);
    }

    // DELETE api/<CategoryController>/5
    [HttpDelete("{id}")]
    [Authorize]
    [Authorize(Policy = "EmployeeOnly")]
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _categoryService.GetAsync(id);
        category.Deleted = true;
        var result = await _categoryService.UpdateAsync(category);

        return Ok(result);
    }
}
