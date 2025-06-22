using System.Text.Json;
using MediatR;
using ShoppingCart.Application.Producer;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Models;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Application.QueryHandlers;

public class ProcessCartToOrderHandler : IRequestHandler<ProcessCartToOrderCommand, OrderDto>
{
    private readonly ICartRepository _repository;
    private readonly IKafkaProducer _kafkaProducer;

    public ProcessCartToOrderHandler(ICartRepository repository, IKafkaProducer kafkaProducer)
    {
        _repository = repository;
        _kafkaProducer = kafkaProducer;
    }

    public Task<OrderDto> Handle(ProcessCartToOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = _repository.FindById(request.CartId);
        if (cart == null)
            return Task.FromResult<OrderDto>(null);

        var items = cart.Items.Select(i => new Item
        {
            Id = i.Id,
            Name = i.Name,
            Price = i.Price,
            Quantity = i.Quantity,
        }).ToList();

        var totalPrice = items.Sum(i => i.Price * i.Quantity);

        var order = new OrderDto
        {
            CartId = request.CartId,
            Email = request.Email,
            Items = items,
            TotalPrice = totalPrice
        };

        string fileName = $"OrderInfo_Id_{request.CartId}_Email_{request.Email}.json";
        var options = new JsonSerializerOptions { WriteIndented = true };
        string order_json = JsonSerializer.Serialize(order, options);
        File.WriteAllText(fileName, order_json);

        _kafkaProducer.SendMessageAsync("order-processed-email-topic", order_json);

        return Task.FromResult(order);
    }
}
