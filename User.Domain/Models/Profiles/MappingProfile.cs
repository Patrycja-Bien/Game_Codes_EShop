using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Response;
using AutoMapper;

namespace User.Domain.Models.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.LastLoginAt, opt => opt.MapFrom(src => src.LastLoginAt))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));
    }
}
