using AutoMapper;
using Db.Entity;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class MenuItemProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MenuItem, MenuItemViewModel>();
            Mapper.CreateMap<MenuItemViewModel, MenuItem>();
        }
    }
}