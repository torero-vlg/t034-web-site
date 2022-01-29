using System.Linq;
using AutoMapper;
using T034.Core.Dto;
using T034.Core.Entity.Administration;

namespace T034.Core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
                  //.ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => AutoMapperWebConfiguration.IdsToString(src.UserRoles)));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserRoles,
                       opt => opt.MapFrom(src => src.UserRoles.Where(ur => ur.Selected)))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore());
        }
    }
}