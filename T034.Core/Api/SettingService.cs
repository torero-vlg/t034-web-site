using System.Collections.Generic;
using T034.Core.Api.Common;
using T034.Core.Entity;
using T034.Core.Entity.Administration;
using NLog;
using T034.Core.DataAccess;

namespace T034.Core.Api
{
    public interface ISettingService
    {
        /// <summary>
        /// Создать пользователя Администратор
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User CreateFirstUser(string email);

        /// <summary>
        /// Обновить идентификатор приложения Яндекс
        /// </summary>
        /// <returns></returns>
        bool UpdateYandexClientId(string clientId);

        /// <summary>
        /// Обновить пароль приложения Яндекс
        /// </summary>
        /// <returns></returns>
        bool UpdateYandexPassword(string password);
        
        /// <summary>
        /// Получить настройку "Стартовая страница"
        /// </summary>
        /// <returns></returns>
        Setting GetStartPage();

        /// <summary>
        /// Инициализация настроек
        /// </summary>
        /// <returns></returns>
        void Init();

        /// <summary>
        /// Все настройки
        /// </summary>
        /// <returns></returns>
        IEnumerable<Setting> Settings();

        /// <summary>
        /// Получить настройку
        /// </summary>
        /// <returns></returns>
        Setting Get(int id);

        /// <summary>
        /// Обновить настройку
        /// </summary>
        /// <returns></returns>
        Setting Save(Setting item);
    }

    public class SettingService : AbstractService, ISettingService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public SettingService(IBaseDb db) 
            : base(db)
        { 
        }

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

        public void Init()
        {
            //инициализация настроек
            if (Db.SingleOrDefault<Setting>(s => s.Code == "StartPage") == null)
                Db.SaveOrUpdate(new Setting { Code = "StartPage", Name = "Стартовая страница", Value = "" });

            if (Db.SingleOrDefault<Setting>(s => s.Code == "YandexClientId") == null)
                Db.SaveOrUpdate(new Setting { Code = "YandexClientId", Name = "YandexClientId", Value = "" });

            if (Db.SingleOrDefault<Setting>(s => s.Code == "YandexPassword") == null)
                Db.SaveOrUpdate(new Setting { Code = "YandexPassword", Name = "YandexPassword", Value = "" });

            //инициализация ролей
            if (Db.SingleOrDefault<Role>(u => u.Code == "Administrator") == null)
                Db.SaveOrUpdate(new Role { Code = "Administrator", Name = "Администратор" });

            if (Db.SingleOrDefault<Role>(u => u.Code == "Moderator") == null)
                Db.SaveOrUpdate(new Role { Code = "Moderator", Name = "Модератор" });

            _logger.Info("Инициализации настроек выполнена");
        }

        public IEnumerable<Setting> Settings()
        {
            return Db.Select<Setting>();
        }

        public Setting Get(int id)
        {
            return Db.Get<Setting>(id);
        }

        public Setting Save(Setting item)
        {
            Db.SaveOrUpdate(item);
            return item;
        }
    }
}
