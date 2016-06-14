using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Db.Api;
using Db.Entity;
using Db.Entity.Administration;
using Ninject;
using T034.Tools.Attribute;
using T034.ViewModel;

namespace T034.Controllers
{
    public class SettingController : BaseController
    {
        [Inject]
        public ISettingService SettingService { get; set; }
        [Inject]
        public IUserService UserService { get; set; }

        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var items = Db.Select<Setting>();

                var model = new List<SettingViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }

        [HttpGet]
        [Role("Administrator")]
        public ActionResult AddOrEdit(int? id)
        {
            var model = new SettingViewModel();
            if (id.HasValue)
            {
                var item = Db.Get<Setting>(id.Value);
                model = Mapper.Map(item, model);
            }

            return View(model);
        }

        [Role("Administrator")]
        public ActionResult AddOrEdit(SettingViewModel model)
        {
            var item = new Setting();
            if (model.Id > 0)
            {
                item = Db.Get<Setting>(model.Id);
            }
            item = Mapper.Map(model, item);

            var result = Db.SaveOrUpdate(item);

            return RedirectToAction("List");
        }

        public ActionResult Index()
        {
            //TODO Api
            //инициализация настроек
            if (Db.SingleOrDefault<Setting>(s => s.Code == "StartPage") == null)
                Db.SaveOrUpdate(new Setting { Code = "StartPage", Name = "Стартовая страница", Value = ""});

            if (Db.SingleOrDefault<Setting>(s => s.Code == "YandexClientId") == null)
                Db.SaveOrUpdate(new Setting { Code = "YandexClientId", Name = "YandexClientId", Value = "" });

            if (Db.SingleOrDefault<Setting>(s => s.Code == "YandexPassword") == null)
                Db.SaveOrUpdate(new Setting { Code = "YandexPassword", Name = "YandexPassword", Value = "" });

            //инициализация ролей
            if (Db.SingleOrDefault<Role>(u => u.Code == "Administrator") == null)
                Db.SaveOrUpdate(new Role { Code = "Administrator", Name = "Администратор" });

            if (Db.SingleOrDefault<Role>(u => u.Code == "Moderator") == null)
                Db.SaveOrUpdate(new Role { Code = "Moderator", Name = "Модератор" });

            //инициализация папок
            var directory = new DirectoryInfo(Server.MapPath(string.Format("/{0}", MvcApplication.FilesFolder)));
            if(!directory.Exists)
                directory.Create();

            directory = new DirectoryInfo(Server.MapPath(string.Format("/{0}", "Upload")));
            if (!directory.Exists)
                directory.Create();

            directory = new DirectoryInfo(Server.MapPath(string.Format("/{0}", "Upload/Images")));
            if (!directory.Exists)
                directory.Create();

            //если ни одного пользователя в БД, то показваем форму с Email и полями для OAuth
            if(!Db.Select<User>().Any())
                return View("CreateUserAndOAuth");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateUserAndOAuth(FirstUserViewModel model)
        {
            if (UserService.GetUser(model.Email) == null)
            {
                SettingService.CreateFirstUser(model.Email);

                SettingService.UpdateYandexClientId(model.YandexClientId);
                SettingService.UpdateYandexPassword(model.YandexPassword);

                return RedirectToAction("Logon", "Account");
            }

            return RedirectToAction("List");
        }
    }
}
