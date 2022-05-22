using System.Collections.Generic;
using T034.Core.Api.Common;
using T034.Core.DataAccess;
using T034.Core.Entity.Administration;

namespace T034.Core.Api
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
        public UserService(IBaseDb db)
            : base(db)
        {
        }

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
