using Db.Api.Common;
using Db.Entity;
using Db.Entity.Administration;
using NLog;

namespace Db.Api
{
    public interface ISettingService
    {
        User CreateFirstUser(string email);
        bool UpdateYandexClientId(string clientId);
        bool UpdateYandexPassword(string password);
        Setting GetStartPage();
    }

    public class SettingService : AbstractService, ISettingService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
                _logger.Info($"Создан первый пользователь '{user}'");
            }
            else
            {
                _logger.Warn($"Пользователь '{user}' уже существует");
            }
            return user;
        }

        public bool UpdateYandexClientId(string clientId)
        {
            var setting = Db.SingleOrDefault<Setting>(s => s.Code == "YandexClientId");
            setting.Value = clientId;
            Db.SaveOrUpdate(setting);
            _logger.Info("Обновлён идентификатор приложения Яндекс");
            return true;
        }

        public bool UpdateYandexPassword(string password)
        {
            var setting = Db.SingleOrDefault<Setting>(s => s.Code == "YandexPassword");
            setting.Value = password;
            Db.SaveOrUpdate(setting);
            _logger.Info("Обновлён пароль приложения Яндекс");
            return true;
        }

        public Setting GetStartPage()
        {
            var item = Db.SingleOrDefault<Setting>(s => s.Code == "StartPage");
            return item;
        }
    }
}
