using T034.Core.DataAccess;
using T034.Core.Entity.Administration;

namespace T034.Repository
{
    public class Repository : IRepository
    {
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