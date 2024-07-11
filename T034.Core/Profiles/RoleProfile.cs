using AutoMapper;
using T034.Core.Dto;
using T034.Core.Entity.Administration;

namespace T034.Core.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(vm => vm.Selected, v => v.MapFrom(src => true));
            CreateMap<RoleDto, Role>();
        }
    }
}