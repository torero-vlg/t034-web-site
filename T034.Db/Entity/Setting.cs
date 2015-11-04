namespace Db.Entity
{
    public class Setting : Entity
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
    }
}
