using Db.DataAccess;
using Db.Entity.Administration;
using Ninject;

namespace T034.Repository
{
    public class Repository : IRepository
    {
        [Inject]
        protected IBaseDb Db { get; set; }

        public User Login(string email, string password)
        {
            return Db.SingleOrDefault<User>(p => string.Compare(p.Email, email, true) == 0 && p.Password == password);
        }

        public User GetUser(string email)
        {
            return Db.SingleOrDefault<User>(p => string.Compare(p.Email, email, true) == 0);
        }
    }
}