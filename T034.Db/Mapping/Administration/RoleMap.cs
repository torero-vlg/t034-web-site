using Db.Entity.Administration;
using FluentNHibernate.Mapping;

namespace Db.Mapping.Administration
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(x => x.Id).Column("RoleId").GeneratedBy.Increment();

            Map(p => p.Name);
            Map(p => p.Code);
        }
    }
}
