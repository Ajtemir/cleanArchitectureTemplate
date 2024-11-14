using Application.Common.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(x => x.PhotoBase64, c => c.MapFrom(s =>
                s.Photo == null ? null : Convert.ToBase64String(s.Photo)))
            .ForMember(x => x.Roles, c => c.MapFrom(s =>
                s.UserRoles.Select(userRole => userRole.Role.Name).ToList()));
    }
}
