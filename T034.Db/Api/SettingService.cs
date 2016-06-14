using Db.Api.Common;
using Db.Entity;
using Db.Entity.Administration;

namespace Db.Api
{
    public interface ISettingService
    {
        User CreateFirstUser(string email);
        bool UpdateYandexClientId(string clientId);
        bool UpdateYandexPassword(string password);
    }

    public class SettingService : AbstractService, ISettingService
    {
        public User CreateFirstUser(string email)
        {
            var user = Db.SingleOrDefault<User>(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Name = "Администратор",
                    Email = email,
                    UserRoles = Db.Select<Role>()
                };
                Db.SaveOrUpdate(user);
                Logger.Info($"Создан первый пользователь '{user}'");
            }
            else
            {
                Logger.Warn($"Пользователь '{user}' уже существует");
            }
            return user;
        }

        public bool UpdateYandexClientId(string clientId)
        {
            var setting = Db.SingleOrDefault<Setting>(s => s.Code == "YandexClientId");
            setting.Value = clientId;
            Db.SaveOrUpdate(setting);
            Logger.Info("Обновлён идентификатор приложения Яндекс");
            return true;
        }

        public bool UpdateYandexPassword(string password)
        {
            var setting = Db.SingleOrDefault<Setting>(s => s.Code == "YandexPassword");
            setting.Value = password;
            Db.SaveOrUpdate(setting);
            Logger.Info("Обновлён пароль приложения Яндекс");
            return true;
        }
    }
}
