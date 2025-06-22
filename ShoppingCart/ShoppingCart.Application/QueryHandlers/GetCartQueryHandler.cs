using Azure.Core;
using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Queries;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Application.QueryHandlers;

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Cart>
{
    private readonly ICartReader _cartReader;
    private readonly ICartRepository _repository;

    public GetCartQueryHandler(ICartReader cartReader, ICartRepository repository)
    {
        _cartReader = cartReader;
        _repository = repository;
    }

    public Task<Cart> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = _repository.FindById(request.CartId);
        if (cart == null)
            return Task.FromResult<Cart>(null);

        var items = cart.Items.Select(i => new Item
        {
            Id = i.Id,
            Name = i.Name,
            Price = i.Price,
            Quantity = i.Quantity,
        }).ToList();

        var totalPrice = items.Sum(i => i.Price * i.Quantity);

        var Cart = new Cart
        {
            Id = request.CartId,
            Items = items,
            TotalPrice = totalPrice
        };

        return Task.FromResult(Cart);
    }
}
