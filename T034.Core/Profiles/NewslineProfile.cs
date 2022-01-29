using AutoMapper;
using T034.Core.Dto;
using T034.Core.Entity;

namespace Db.Profiles
{
    public class NewslineProfile : Profile
    {
        public NewslineProfile()
        {
            CreateMap<Newsline, NewslineDto>();
            CreateMap<NewslineDto, Newsline>();
        }
    }
}