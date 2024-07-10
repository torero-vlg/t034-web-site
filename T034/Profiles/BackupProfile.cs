using System;
using AutoMapper;
using T034.Core.Dto;
using T034.ViewModel;

namespace T034.Profiles
{
    public class BackupProfile : Profile
    {
        public BackupProfile()
        {
            CreateMap<BackupDto, BackupViewModel>()
                    .ForMember(dest => dest.Size, opt => opt.MapFrom(src => SizeSuffix(src.Size)));
        }

        static readonly string[] SizeSuffixes =
                { "Б", "Кбайт", "Мбайт", "Гбайт", "Тбайт", "Пбайт", "Эбайт", "Збайт", "Ибайт" };

        static string SizeSuffix(long value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return $"{dValue:n1} {SizeSuffixes[i]}";
        }
    }
}