using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Application.Services;


public class CartService : ICartAdder, ICartRemover, ICartReader
{
    private readonly ICartRepository _repository;

    public CartService(ICartRepository repository)
    {
        _repository = repository;
    }

    public void AddItemToCart(int cartId, Item item)
    {
        var cart = _repository.FindById(cartId);

        if (cart == null)
        {
            cart = new Cart { Id = cartId };
            cart.Items.Add(item);
            _repository.Add(cart); 
        }
        else
        {
            cart.Items.Add(item);
            _repository.Update(cart);
        }
    }


    public void RemoveItemFromCart(int cartId, int itemId)
    {
        var cart = _repository.FindById(cartId);
        if (cart != null)
        {
            var item = cart.Items.FirstOrDefault(p => p.Id == itemId);
            if (item != null)
            {
                cart.Items.Remove(item);
                _repository.Update(cart);
            }
        }
    }

    public Cart GetCart(int cartId)
    {
        var cart = _repository.FindById(cartId);
        if (cart == null) return null;

        return new Cart
        {
            Id = cart.Id,
            Items = cart.Items.Select(p => new Item
            {
                Id = p.Id
            }).ToList(),
            TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity)
        };
    }

    public List<Cart> GetAllCarts()
    {
        return _repository.GetAll().Select(c => new Cart
        {
            Id = c.Id,
            Items = c.Items.Select(p => new Item
            {
                Id = p.Id
            }).ToList(),
            TotalPrice = c.Items.Sum(i => i.Price * i.Quantity)
        }).ToList();
    }
}
