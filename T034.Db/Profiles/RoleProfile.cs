using AutoMapper;
using Db.Dto;
using Db.Entity.Administration;

namespace Db.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(vm => vm.Selected, v => v.UseValue(true));
            CreateMap<RoleDto, Role>();
        }
    }
}