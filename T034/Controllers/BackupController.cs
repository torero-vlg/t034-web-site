using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Api;
using T034.Core.Entity;
using T034.Tools.Attribute;
using T034.ViewModel;
using Microsoft.AspNetCore.Hosting;
using T034.Core.DataAccess;
using T034.Core.Services.Common;

namespace T034.Controllers
{
    public class BackupController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;
        private readonly IBackupService _backupService;

        public BackupController(IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IUserService userService,
            IBaseDb db,
            IBackupService backupService) 
            : base(db)
        {
            _webHostEnvironment = webHostEnvironment;
            _settingService = settingService;
            _userService = userService;
            _backupService = backupService;
        }
        
        [Role("Administrator")]
        public ActionResult List()
        {
            try
            {
                var items = _backupService.GetBackups();

                var model = new List<BackupViewModel>();
                model = Mapper.Map(items, model);

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return View("ServerError", (object)"Получение списка");
            }
        }
    }
}
