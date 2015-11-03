using Db.Entity;
using FluentNHibernate.Mapping;

namespace Db.Mapping
{
    public class PageMap : ClassMap<Page>
    {
        public PageMap()
        {
            Id(x => x.Id).Column("PageId").GeneratedBy.Increment();

            Map(p => p.Comment);
            Map(p => p.Content);
            Map(p => p.Title);
        }
    }
}
