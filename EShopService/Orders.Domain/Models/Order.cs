using System.ComponentModel.DataAnnotations;
using EShop.Domain.Models;
using Orders.Domain.Enums;

namespace Orders.Domain.Models;

public class Order : BaseModel
{
    [Key]
    public int Id { get; set; }

    public List<CartItem> CartItems { get; set; } = new();

    public decimal TotalAmount { get; set; } = 0;

    public string Currency { get; set; } = "PLN";

    public OrderStatus Status { get; set; } = OrderStatus.New;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public string ShippingMethod { get; set; } = string.Empty;

    public Address Address { get; set; } = default!;

    public string Notes { get; set; } = string.Empty;
}
