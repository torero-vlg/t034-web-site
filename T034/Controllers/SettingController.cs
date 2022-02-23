using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Api;
using T034.Core.Entity;
using OAuth2;
using T034.Tools.Attribute;
using T034.ViewModel;
using Microsoft.AspNetCore.Hosting;
using T034.Core.DataAccess;

namespace T034.Controllers
{
    public class SettingController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;

        public SettingController(AuthorizationRoot authorizationRoot, 
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IUserService userService,
            IBaseDb db) 
            : base(authorizationRoot, db)
        {
            _webHostEnvironment = webHostEnvironment;
            _settingService = settingService;
            _userService = userService;
        }
        
        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var items = _settingService.Settings();

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
                var item = _settingService.Get(id.Value);
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
                item = _settingService.Get(model.Id);
            }
            item = Mapper.Map(model, item);

            var result = _settingService.Save(item);

            return RedirectToAction("List");
        }

        public ActionResult Index()
        {
            _settingService.Init();

            //инициализация папок
            string webRootPath = _webHostEnvironment.WebRootPath;

            var directory = new DirectoryInfo(Path.Combine(webRootPath, $"/{Program.FilesFolder}"));
            if(!directory.Exists)
                directory.Create();

            directory = new DirectoryInfo(Path.Combine(webRootPath, $"/Upload"));
            if (!directory.Exists)
                directory.Create();

            directory = new DirectoryInfo(Path.Combine(webRootPath, $"/Upload/Images"));
            if (!directory.Exists)
                directory.Create();

            //если ни одного пользователя в БД, то показваем форму с Email и полями для OAuth
            if(!_userService.Users().Any())
                return View("CreateUserAndOAuth");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateUserAndOAuth(FirstUserViewModel model)
        {
            if (_userService.GetUser(model.Email) == null)
            {
                _settingService.CreateFirstUser(model.Email);

                _settingService.UpdateYandexClientId(model.YandexClientId);
                _settingService.UpdateYandexPassword(model.YandexPassword);

                return RedirectToAction("Logon", "Account");
            }

            return RedirectToAction("List");
        }
    }
}
