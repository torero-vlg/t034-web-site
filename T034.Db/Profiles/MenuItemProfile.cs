using AutoMapper;
using Db.Dto;
using Db.Entity;

namespace Db.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, MenuItemDto>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Parent == null ? null : (int?)src.Parent.Id))
                .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.Parent == null ? "" : src.Parent.ToString()));

            CreateMap<MenuItemDto, MenuItem>()
                .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.ParentId.HasValue ? new MenuItem { Id = src.ParentId.Value } : null));
        }
    }
}