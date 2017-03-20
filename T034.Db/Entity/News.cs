using System;
using System.Collections.Generic;
using Db.Entity.Administration;

namespace Db.Entity
{
    public class News : Entity
    {
        public virtual string Body { get; set; }
        
        public virtual string Title { get; set; }
        
        public virtual string Resume { get; set; }

        public virtual DateTime LogDate { get; set; }

        public virtual User User { get; set; }

        public virtual Newsline Newsline { get; set; }
    }
}
