using Microsoft.AspNetCore.Mvc;
using Game_Codes_EShop_Domain.Models;
using Game_Codes_EShop_Application.Services;

namespace Game_Codes_EShopService.Controllers;

public class GameController : ControllerBase
{
    private IGame_Service _game_service;
    public GameController(IGame_Service game_service)
    {
        _game_service = game_service;
    }

    // GET: api/<GameController>
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var result = await _game_service.GetAllAsync();
        return Ok(result);
    }

    // GET api/<GameController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(int id)
    {
        var result = await _game_service.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST api/<GameController>
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] Game game)
    {
        var result = await _game_service.Add(game);

        return Ok(result);
    }

    // PUT api/<GameController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] Game game)
    {
        var result = await _game_service.UpdateAsync(game);

        return Ok(result);
    }

    // DELETE api/<GameController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var game = await _game_service.GetAsync(id);
        game.Deleted = true;
        var result = await _game_service.UpdateAsync(game);

        return Ok(result);
    }
}
