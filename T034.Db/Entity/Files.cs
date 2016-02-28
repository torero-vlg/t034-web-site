using System;
using Db.Entity.Administration;

namespace Db.Entity
{
    public class Files : Entity
    {
        public virtual string Name { get; set; }
        public virtual int DownloadCounter { get; set; }
        
        public virtual Folder Folder { get; set; }
        
        public virtual DateTime LogDate { get; set; }
        public virtual int Size { get; set; }
        public virtual User User { get; set; }
    }
}
