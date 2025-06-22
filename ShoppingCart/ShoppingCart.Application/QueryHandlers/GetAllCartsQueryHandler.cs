using Azure.Core;
using MediatR;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Queries;
using ShoppingCart.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.QueryHandlers;

public class GetAllCartsQueryHandler : IRequestHandler<GetAllCartsQuery, List<Cart>>
{
    private readonly ICartReader _cartReader;
    private readonly ICartRepository _repository;

    public GetAllCartsQueryHandler(ICartReader cartReader, ICartRepository repository)
    {
        _cartReader = cartReader;
        _repository = repository;
    }

    public Task<List<Cart>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
    {
        var carts = _repository.GetAll();
        if (carts == null)
            return Task.FromResult<List<Cart>>(null);

        var cart_list = new List<Cart>();

        foreach (var cart in carts)
        {
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
                Id = cart.Id,
                Items = items,
                TotalPrice = totalPrice
            };
            cart_list.Add(Cart);
        }

        return Task.FromResult(cart_list);
    }
}
