using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Domain.Repositories;

class Repository : IRepository
{
    private readonly DataContext _context;

    public Repository(DataContext dataContext)
    {
        _context = dataContext;
    }

    public async Task<User.Domain.Models.UserRequest> AddUserAsync(User.Domain.Models.UserRequest user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<List<User.Domain.Models.UserRequest>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User.Domain.Models.UserRequest> GetUserAsync(int id)
    {
        return await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User.Domain.Models.UserRequest> UpdateUserAsync(User.Domain.Models.UserRequest user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}