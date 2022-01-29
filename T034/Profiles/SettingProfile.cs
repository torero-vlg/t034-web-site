using AutoMapper;
using Db.Entity;
using T034.ViewModel;

namespace T034.Profiles
{
    public class SettingProfile : Profile
    {
        public SettingProfile()
        {
            CreateMap<Setting, SettingViewModel>();
            CreateMap<SettingViewModel, Setting>();
        }
    }
}