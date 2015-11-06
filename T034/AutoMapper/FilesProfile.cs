using AutoMapper;
using Db.Entity;
using T034.ViewModel;

namespace T034.AutoMapper
{
    public class FilesProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Files, FileViewModel>();
            Mapper.CreateMap<FileViewModel, Files>();

            Mapper.CreateMap<Folder, FolderViewModel>()
                .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolder == null ? (int?)null : src.ParentFolder.Id));
            Mapper.CreateMap<FolderViewModel, Folder>()
                .ForMember(dest => dest.ParentFolder, opt => opt.MapFrom(src => src.ParentFolderId.HasValue ? new Folder {Id = src.ParentFolderId.Value} : null));
        }
    }
}