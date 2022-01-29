using T034.Core.Entity;
using FluentNHibernate.Mapping;

namespace T034.Core.Mapping
{
    public class NewslineMap : ClassMap<Newsline>
    {
        public NewslineMap()
        {
            Id(x => x.Id).Column("NewslineId").GeneratedBy.Increment();

            Map(p => p.Name);
            HasMany(x => x.News).KeyColumn("NewslineId").LazyLoad();
        }
    }
}
