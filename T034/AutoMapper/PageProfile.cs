using System.Web;
using AutoMapper;
using Db.Entity;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class PageProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Page, PageViewModel>()
                  .ForMember(dest => dest.Content, opt => opt.MapFrom(src => HttpUtility.HtmlDecode(src.Content)));

            Mapper.CreateMap<PageViewModel, Page>();
                  //.ForMember(dest => dest.Content, opt => opt.MapFrom(src => Sanitizer.GetSafeHtmlFragment(src.Content)));
                 // .ForMember(dest => dest.Content, opt => opt.MapFrom(src => AutoMapperWebConfiguration.GetSafeHtml(src.Content)));
        }
    }
}