using AutoMapper;
using NotifyHub.Domain.Entities;
using NotifyHub.Application.Requests.User;
using NotifyHub.Domain.DTOs;

namespace NotifyHub.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserRequest, User>();
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Notifications, opt => opt.Ignore());
    }
}
