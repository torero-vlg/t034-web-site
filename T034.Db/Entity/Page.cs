using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Db.Entity
{
    public class Page : Entity
    {
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual string Comment { get; set; }
    }
}
