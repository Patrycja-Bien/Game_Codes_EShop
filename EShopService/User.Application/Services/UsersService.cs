using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Repositories;
using User.Domain.Models;
using AutoMapper;

namespace User.Application.Services;

public class UsersService
{
    private readonly IRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersService(IRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetUserDataAsync(int userId)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (user == null)
            return null;

        return _mapper.Map<UserDto>(user);
    }
}
