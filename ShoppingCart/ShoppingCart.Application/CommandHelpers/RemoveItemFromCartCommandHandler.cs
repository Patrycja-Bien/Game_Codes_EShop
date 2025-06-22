using MediatR;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.CommandHelpers;


public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand>
{
    private readonly ICartRemover _cartRemover;

    public RemoveItemFromCartCommandHandler(ICartRemover cartRemover)
    {
        _cartRemover = cartRemover;
    }

    public Task Handle(RemoveItemFromCartCommand command, CancellationToken cancellationToken)
    {
        _cartRemover.RemoveItemFromCart(command.CartId, command.ItemId);
        return Task.CompletedTask;
    }
}
