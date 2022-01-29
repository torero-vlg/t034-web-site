using T034.Core.Entity;
using FluentNHibernate.Mapping;

namespace T034.Core.Mapping
{
    public class MenuItemMap : ClassMap<MenuItem>
    {
        public MenuItemMap()
        {
            Id(x => x.Id).Column("MenuItemId").GeneratedBy.Increment();

            Map(p => p.Url);
            Map(p => p.Title);
            Map(p => p.OrderIndex);

            References(p => p.Parent).Column("ParentId")
                .Not.LazyLoad();
        }
    }
}
