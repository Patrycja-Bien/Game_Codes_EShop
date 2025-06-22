using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.CommandHelpers;

public class AddProductToCartCommandHandler : IRequestHandler<AddItemToCartCommand>
{
    private readonly ICartAdder _cartAdder;

    public AddProductToCartCommandHandler(ICartAdder cartAdder)
    {
        _cartAdder = cartAdder;
    }

    public Task Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        var item = new Item
        {
            Id = command.ItemId,
            Name = command.ItemName,
            Quantity = command.Quantity,
            Price = command.Price
        };

        _cartAdder.AddItemToCart(command.CartId, item);
        return Task.CompletedTask;

    }
}
