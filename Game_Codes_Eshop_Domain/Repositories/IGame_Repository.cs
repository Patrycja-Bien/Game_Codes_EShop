using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Codes_EShop_Domain.Models;

namespace Game_Codes_EShop_Domain.Repositories;

public interface IGame_Repository
{
    #region Game
    Task<Game> GetGameAsync(int id);
    Task<Game> AddGameAsync(Game game);
    Task<Game> UpdateGameAsync(Game game);
    Task<List<Game>> GetAllGamesAsync();
    #endregion

}
