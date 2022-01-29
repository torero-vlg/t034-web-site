using AutoMapper;
using Db.Dto;
using Db.Entity;

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