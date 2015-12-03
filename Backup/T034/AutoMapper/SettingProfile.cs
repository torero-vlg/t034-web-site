using AutoMapper;
using Db.Entity;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class SettingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Setting, SettingViewModel>();
            Mapper.CreateMap<SettingViewModel, Setting>();
        }
    }
}