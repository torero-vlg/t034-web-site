using AutoMapper;
using Db.Entity.Administration;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class RoleProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Role, RoleViewModel>()
                .ForMember(vm => vm.Selected, v => v.UseValue(true));
            Mapper.CreateMap<RoleViewModel, Role>();
        }
    }
}