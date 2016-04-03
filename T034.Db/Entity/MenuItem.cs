namespace Db.Entity
{
    public class MenuItem : Entity
    {
        public virtual string Url { get; set; }
        public virtual string Title { get; set; }
        public virtual int OrderIndex { get; set; }
        public virtual MenuItem Parent { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
