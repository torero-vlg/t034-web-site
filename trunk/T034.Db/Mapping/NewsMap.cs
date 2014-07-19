using Db.Entity;
using FluentNHibernate.Mapping;

namespace Db.Mapping
{
    public class NewsMap : ClassMap<News>
    {
        public NewsMap()
        {
            
            Id(x => x.Id).Column("NewsId").GeneratedBy.Increment();

            Map(p => p.Body);
        }
    }
}
