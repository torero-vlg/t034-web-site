using System.Linq;
using AutoMapper;
using Db.Dto;
using T034.ViewModel;

namespace T034.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserViewModel>();
                  //.ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => AutoMapperWebConfiguration.IdsToString(src.UserRoles)));

            CreateMap<UserViewModel, UserDto>()
            .ForMember(dest => dest.UserRoles,
                       opt => opt.MapFrom(src => src.UserRoles.Where(ur => ur.Selected)));
        }
    }
}