using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using T034.Core.Dto;
using T034.ViewModel;

namespace T034.Profiles
{
    public class NewslineProfile : Profile
    {
        public NewslineProfile()
        {
            CreateMap<NewslineDto, NewslineViewModel>();
            CreateMap<NewslineViewModel, NewslineDto>();

            CreateMap<NewslineDto, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
        }
    }
}