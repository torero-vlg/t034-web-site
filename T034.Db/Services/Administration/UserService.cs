using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Db.DataAccess;
using Db.Dto;
using Db.Entity.Administration;
using Db.Services.Common;
using Ninject;

namespace Db.Services.Administration
{
    public interface IUserService : IService
    {
        User Create(string name, string email, string password);
        AuthenticateResult Authenticate(string email, string password);
        User Update(UserDto dto);
        IEnumerable<UserDto> Select();
        UserDto Get(int id);
    }

    public class UserService : IUserService
    {
        [Inject]
        public IBaseDb Db { get; set; }

        public User Create(string name, string email, string password)
        {
            var user = new User(email, name, password);

            var result = Db.SaveOrUpdate(user);
            return user;
        }

        public AuthenticateResult Authenticate(string email, string password)
        {
            var result = new AuthenticateResult();
            //найти пользователя
            var user = Db.Where<User>(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                result.IsAuthenticated = false;
                result.Message = "Пользователь не найден";
                return result;
            }

            //проверить пароль
            var isAuthenticated = user.IsValidPassword(password);
            if (isAuthenticated)
            {
                result.IsAuthenticated = true;
                result.User = user;
            }
            else
            {
                result.IsAuthenticated = false;
                result.Message = "Неверный пароль";
            }

            return result;
        }

        public User Update(UserDto dto)
        {
            var item = new User();
            item = Db.Get<User>(dto.Id);
            item = Mapper.Map(dto, item);

            if(!string.IsNullOrEmpty(dto.Password))
            {
                item.ChangePassword(dto.Password);
            }

            var result = Db.SaveOrUpdate(item);

            return item;
        }

        public IEnumerable<UserDto> Select()
        {
            var list = new List<UserDto>();
            var items = Db.Select<User>();
            list = Mapper.Map(items, list);
            return list;
        }

        public UserDto Get(int id)
        {
            var dto = new UserDto();
            var item = Db.Get<User>(id);
            dto = Mapper.Map(item, dto);
            return dto;
        }
    }

    public class AuthenticateResult
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
