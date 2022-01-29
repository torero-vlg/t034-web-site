using T034.Core.Entity;
using FluentNHibernate.Mapping;

namespace T034.Core.Mapping
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
