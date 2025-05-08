using Game_Codes_EShop_Domain.Models;

namespace Game_Codes_EShop_Domain.Models;


public class Game : Base_Model
{
    public int Id { get; set; }

    public string Game_Name { get; set; } = string.Empty;

    public string Game_Code_Value { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Stock { get; set; } = 0;

    public string Sku { get; set; } = string.Empty;

    public Category Game_Category { get; set; } = default!;

}
