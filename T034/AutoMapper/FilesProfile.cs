using System;
using AutoMapper;
using Db.Entity;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class FilesProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Files, FileViewModel>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                    .ForMember(dest => dest.Size, opt => opt.MapFrom(src => SizeSuffix(src.Size)))
                    .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name));
            Mapper.CreateMap<FileViewModel, Files>();

            Mapper.CreateMap<Folder, FolderViewModel>()
                .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolder == null ? (int?)null : src.ParentFolder.Id));
            Mapper.CreateMap<FolderViewModel, Folder>()
                .ForMember(dest => dest.ParentFolder, opt => opt.MapFrom(src => src.ParentFolderId.HasValue ? new Folder {Id = src.ParentFolderId.Value} : null));
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