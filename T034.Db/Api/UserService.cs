using Db.Api.Common;
using Db.Entity.Administration;

namespace Db.Api
{
    public interface IUserService
    {
        User GetUser(string email);
    }

    public class UserService : AbstractService, IUserService
    {
        public User GetUser(string email)
        {
            var user = Db.SingleOrDefault<User>(u => u.Email == email);
            return user;
        }
    }
}
