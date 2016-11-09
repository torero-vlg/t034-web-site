using AutoMapper;
using Db.Dto;
using Db.Entity.Administration;

namespace Db.AutoMapper
{
    public class RoleProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Role, RoleDto>()
                .ForMember(vm => vm.Selected, v => v.UseValue(true));
            Mapper.CreateMap<RoleDto, Role>();
        }
    }
}