using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Codes_EShop_Domain.Repositories;
using Game_Codes_EShop_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Game_Codes_EShop_Domain.Repositories;

public class Game_Repository : IGame_Repository
{
    private readonly DataContext _context;
    public Game_Repository(DataContext dataContext)
    {
        _context = dataContext;
    }
    public async Task<Game> AddGameAsync(Game game)
    {
        _context.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        return await _context.Games.ToListAsync();
    }

    public async Task<Game> GetGameAsync(int id)
    {
       return await _context.Games.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Game> UpdateGameAsync(Game game)
    {
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
        return game;
    }
}
