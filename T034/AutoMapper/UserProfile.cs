using System.Linq;
using AutoMapper;
using Db.Dto;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserDto, UserViewModel>();
                  //.ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => AutoMapperWebConfiguration.IdsToString(src.UserRoles)));

            Mapper.CreateMap<UserViewModel, UserDto>()
            .ForMember(dest => dest.UserRoles,
                       opt => opt.MapFrom(src => src.UserRoles.Where(ur => ur.Selected)));
        }
    }
}