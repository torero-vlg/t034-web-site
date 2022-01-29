using System.Web;
using AutoMapper;
using T034.Core.Entity;
using T034.ViewModel;

namespace T034.Profiles
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<Page, PageViewModel>()
                  .ForMember(dest => dest.Content, opt => opt.MapFrom(src => HttpUtility.HtmlDecode(src.Content)));

            CreateMap<PageViewModel, Page>();
                  //.ForMember(dest => dest.Content, opt => opt.MapFrom(src => Sanitizer.GetSafeHtmlFragment(src.Content)));
                 // .ForMember(dest => dest.Content, opt => opt.MapFrom(src => AutoMapperWebConfiguration.GetSafeHtml(src.Content)));
        }
    }
}