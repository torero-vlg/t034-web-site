using AutoMapper;
using Db.Entity;
using Db.Entity.Administration;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class NewsProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<News, NewsViewModel>()
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));

            Mapper.CreateMap<NewsViewModel, News>()
                .ForMember(dest => dest.User,
                       opt => opt.MapFrom(src => new User { Id = src.UserId }));
        }
    }
}