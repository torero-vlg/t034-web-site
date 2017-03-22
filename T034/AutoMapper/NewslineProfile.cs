using System.Web.Mvc;
using AutoMapper;
using Db.Dto;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class NewslineProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NewslineDto, NewslineViewModel>();
            Mapper.CreateMap<NewslineViewModel, NewslineDto>();

            Mapper.CreateMap<NewslineDto, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
        }
    }
}