using AutoMapper;
using T034.Core.Dto;
using T034.Core.Entity;
using T034.Core.Entity.Administration;

namespace T034.Core.Profiles
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsDto>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
              .ForMember(dest => dest.NewslineId, opt => opt.MapFrom(src => src.Newsline.Id))
              .ForMember(dest => dest.Newsline, opt => opt.MapFrom(src => src.Newsline.Name));

            CreateMap<NewsDto, News>()
                //.ForMember(dest => dest.Body, opt => opt.MapFrom(src => AutoMapperWebConfiguration.GetSafeHtml(src.Body)))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.UserId }))
                .ForMember(dest => dest.Newsline, opt => opt.MapFrom(src => new Newsline { Id = src.NewslineId }));
        }
    }
}