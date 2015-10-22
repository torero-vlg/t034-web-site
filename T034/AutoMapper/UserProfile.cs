﻿using AutoMapper;
using Db.Entity.Administration;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserViewModel>()
                  .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => AutoMapperWebConfiguration.IdsToString(src.UserRoles)));

            Mapper.CreateMap<UserViewModel, User>()
                  .ForMember(dest => dest.UserRoles,
                             opt => opt.MapFrom(src => AutoMapperWebConfiguration.StringToCollection<Role>(src.Roles)));
        }
    }
}