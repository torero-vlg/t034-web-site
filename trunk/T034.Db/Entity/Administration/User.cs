namespace Db.Entity.Administration
{
    public class User : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
    }
}
