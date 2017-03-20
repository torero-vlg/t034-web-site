using System.Collections.Generic;

namespace Db.Entity
{
    /// <summary>
    /// Новостная лента
    /// </summary>
    /// <remarks>Для группировки новостей</remarks>
    public class Newsline : Entity
    {
        public virtual string Name { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
