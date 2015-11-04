using Db.Entity;
using FluentNHibernate.Mapping;

namespace Db.Mapping
{
    public class SettingMap : ClassMap<Setting>
    {
        public SettingMap()
        {
            Id(x => x.Id).Column("SettingId").GeneratedBy.Increment();

            Map(p => p.Code);
            Map(p => p.Name);
            Map(p => p.Value);
        }
    }
}
