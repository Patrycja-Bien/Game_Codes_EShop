using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Models;

public class Category : BaseModel
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
