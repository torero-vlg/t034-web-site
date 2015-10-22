using AutoMapper;
using Db.Entity.Administration;

namespace T034.ViewModel.AutoMapper
{
    public class RoleProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Role, RoleViewModel>();
            Mapper.CreateMap<RoleViewModel, Role>();
        }
    }
}