using Db.Entity.Administration;
using FluentNHibernate.Mapping;

namespace Db.Mapping.Administration
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).Column("UserId").GeneratedBy.Increment();

            Map(p => p.Name);
            Map(p => p.Email);
            Map(p => p.Login);
            Map(p => p.Password);
            Map(p => p.Salt);

            HasManyToMany(p => p.UserRoles)
                .Table("UserRole")
                .ParentKeyColumn("UserId")
                .ChildKeyColumn("RoleId")
                .Not.LazyLoad();
        }
    }
}
