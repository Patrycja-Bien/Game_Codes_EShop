using MediatR;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.CommandHelpers;


public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand>
{
    private readonly ICartRemover _cartRemover;

    public RemoveProductFromCartCommandHandler(ICartRemover cartRemover)
    {
        _cartRemover = cartRemover;
    }

    public Task Handle(RemoveProductFromCartCommand command, CancellationToken cancellationToken)
    {
        _cartRemover.RemoveProductFromCart(command.CartId, command.ProductId);
        return Task.CompletedTask;
    }
}
