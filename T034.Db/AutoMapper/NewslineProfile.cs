using AutoMapper;
using Db.Dto;
using Db.Entity;

namespace Db.AutoMapper
{
    public class NewslineProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Newsline, NewslineDto>();
            Mapper.CreateMap<NewslineDto, Newsline>();
        }
    }
}