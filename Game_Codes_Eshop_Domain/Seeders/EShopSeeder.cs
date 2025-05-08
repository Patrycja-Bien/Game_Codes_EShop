using Game_Codes_EShop_Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Codes_EShop_Domain.Models;

namespace Game_Codes_EShop_Domain.Seeders;

public class EShopSeeder(DataContext context) : IEShopSeeder
{
    public async Task Seed()
    {
        if (!context.Games.Any())
        {
            var products = new List<Game>
            {
                new Game { Game_Name = "Game_A"},
                new Game { Game_Name = "Game_B"},
                new Game { Game_Name = "Game_C"}
            };

            context.Games.AddRange(products);
            context.SaveChanges();
            var seededGames = context.Games.ToList();
            foreach (var game in seededGames)
            {
                Console.WriteLine($"Seeded product: {game.Game_Name}");

            }
        }
    }
}
