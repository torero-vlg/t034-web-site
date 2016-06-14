using System;
using System.IO;
using Db.Entity.Administration;

namespace Db.Entity
{
    public class Folder : Entity
    {
        public virtual string Name { get; set; }
        public virtual Folder ParentFolder { get; set; }
        
        public virtual DateTime LogDate { get; set; }
        public virtual User User { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
