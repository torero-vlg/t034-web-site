using AutoMapper;
using T034.Core.Dto;
using T034.ViewModel;

namespace T034.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleDto, RoleViewModel>()
                .ForMember(vm => vm.Selected, v => v.MapFrom(src => true));
            CreateMap<RoleViewModel, RoleDto>();
        }
    }
}