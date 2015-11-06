using Db.Entity;
using FluentNHibernate.Mapping;

namespace Db.Mapping
{
    public class FilesMap : ClassMap<Files>
    {
        public FilesMap()
        {
            Id(x => x.Id).Column("FileId").GeneratedBy.Increment();

            Map(p => p.Name);
            Map(p => p.LogDate);
            Map(p => p.DownloadCounter);
            References(p => p.User).Column("UserId")
                .Not.LazyLoad();
            References(p => p.Folder).Column("FolderId")
                .Not.LazyLoad();
        }
    }
}
