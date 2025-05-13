using System.ComponentModel.DataAnnotations;

namespace EShopDomain.Models;

public class Product : BaseModel
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    //public string Code { get; set; } = string.Empty; atrybut 

    [MaxLength(13)]
    public string Ean { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Stock { get; set; } = 0;

    public Category Category { get; set; } = default!;
}