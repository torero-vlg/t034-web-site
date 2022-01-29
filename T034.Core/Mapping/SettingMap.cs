using T034.Core.Entity;
using FluentNHibernate.Mapping;

namespace T034.Core.Mapping
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
