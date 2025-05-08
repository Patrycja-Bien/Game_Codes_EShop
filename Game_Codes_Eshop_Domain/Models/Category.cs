using Game_Codes_EShop_Domain.Models;

namespace Game_Codes_EShop_Domain.Models;

public class Category : Base_Model
{
    public int Id { get; set; }
    public string Category_Name { get; set; } = string.Empty;
}
