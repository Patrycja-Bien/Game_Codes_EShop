using Game_Codes_EShop_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Codes_EShop_Domain.Repositories;

namespace Game_Codes_EShop_Application.Services;

public class Game_Service : IGame_Service
{
    private IGame_Repository _repository;
    public Game_Service(IGame_Repository repository)
    {
        _repository = repository;
    }
    public Task<Game> Add(Game game)
    {
        return _repository.AddGameAsync(game);
    }

    public async Task<Game> AddAsync(Game game)
    {
        return await _repository.AddGameAsync(game);
    }

    public async Task<List<Game>> GetAllAsync()
    {
        return await _repository.GetAllGamesAsync();
    }

    public async Task<Game> GetAsync(int id)
    {
        return await _repository.GetGameAsync(id);
    }

    public async Task<Game> UpdateAsync(Game game)
    {
        return await _repository.UpdateGameAsync(game);
    }
}
