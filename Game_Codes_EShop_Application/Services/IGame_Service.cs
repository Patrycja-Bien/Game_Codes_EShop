namespace Game_Codes_EShop_Application.Services;
using Game_Codes_EShop_Domain.Models;

public interface IGame_Service
{
    public Task<List<Game>> GetAllAsync();
    Task<Game> GetAsync(int id);
    Task<Game> UpdateAsync(Game game);
    Task<Game> AddAsync(Game game);
    Task<Game> Add(Game game);
}