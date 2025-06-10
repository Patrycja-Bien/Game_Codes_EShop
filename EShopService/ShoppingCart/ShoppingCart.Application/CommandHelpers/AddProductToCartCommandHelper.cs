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

public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>
{
    private readonly ICartAdder _cartAdder;

    public AddProductToCartCommandHandler(ICartAdder cartAdder)
    {
        _cartAdder = cartAdder;
    }

    public Task Handle(AddProductToCartCommand command, CancellationToken cancellationToken)
    {
        var product = new Item
        {
            Id = command.ProductId
        };
        _cartAdder.AddProductToCart(command.CartId, product);
        return Task.CompletedTask;
    }
}
