using System;
using System.Collections.Generic;
using System.Linq;

namespace Db.Entity.Administration
{
    public class User : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual ICollection<Role> UserRoles { get; set; }

        public virtual bool InRoles(string roles)
        {
            if (string.IsNullOrEmpty(roles))
            {
                return false;
            }

            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in rolesArray)
            {
                var hasRole = UserRoles.Any(p => string.Compare(p.Code, role, true) == 0);
                if (hasRole)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Name} [{Email}]";
        }
    }
}
