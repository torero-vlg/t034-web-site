using System.Web.Mvc;
using AutoMapper;
using Db.Entity;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class MenuItemProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MenuItem, MenuItemViewModel>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Parent == null ? null : (int?)src.Parent.Id))
                .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.Parent == null ? "" : src.Parent.ToString()));

            Mapper.CreateMap<MenuItemViewModel, MenuItem>()
                .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.ParentId.HasValue ? new MenuItem { Id = src.ParentId.Value } : null));

            Mapper.CreateMap<MenuItem, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Title + (src.Parent == null ? "" : $" [{src.Parent}]")));
        }
    }
}