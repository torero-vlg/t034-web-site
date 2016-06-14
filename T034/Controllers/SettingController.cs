using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Db.Api;
using Db.Entity;
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
                var items = SettingService.Settings();

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
                var item = SettingService.Get(id.Value);
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
                item = SettingService.Get(model.Id);
            }
            item = Mapper.Map(model, item);

            var result = SettingService.Save(item);

            return RedirectToAction("List");
        }

        public ActionResult Index()
        {
            SettingService.Init();

            //инициализация папок
            var directory = new DirectoryInfo(Server.MapPath($"/{MvcApplication.FilesFolder}"));
            if(!directory.Exists)
                directory.Create();

            directory = new DirectoryInfo(Server.MapPath($"/{"Upload"}"));
            if (!directory.Exists)
                directory.Create();

            directory = new DirectoryInfo(Server.MapPath($"/{"Upload/Images"}"));
            if (!directory.Exists)
                directory.Create();

            //если ни одного пользователя в БД, то показваем форму с Email и полями для OAuth
            if(!UserService.Users().Any())
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
