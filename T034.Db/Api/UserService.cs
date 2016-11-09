using System.Collections.Generic;
using Db.Api.Common;
using Db.Entity.Administration;

namespace Db.Api
{
    public interface IUserService
    {
        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User GetUser(string email);

        /// <summary>
        /// Все пользователи
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> Users();
    }

    public class UserService : AbstractService, IUserService
    {
        public User GetUser(string email)
        {
            var user = Db.SingleOrDefault<User>(u => u.Email == email);
            return user;
        }

        public IEnumerable<User> Users()
        {
            return Db.Select<User>();
        }
    }
}
