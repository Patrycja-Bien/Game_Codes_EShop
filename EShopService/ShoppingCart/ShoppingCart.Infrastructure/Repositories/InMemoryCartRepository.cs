using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Repositories;

public class InMemoryCartRepository : ICartRepository
{
    private readonly List<Cart> _carts = new List<Cart>();

    public void Add(Cart cart)
    {
        _carts.Add(cart);
    }

    public void Update(Cart cart)
    {
        var existingCart = _carts.FirstOrDefault(c => c.Id == cart.Id);
        if (existingCart != null)
        {
            existingCart.Items = cart.Items;
        }
    }

    public Cart FindById(int id)
    {
        return _carts.FirstOrDefault(c => c.Id == id);
    }

    public List<Cart> GetAll()
    {
        return _carts.ToList();
    }
}
