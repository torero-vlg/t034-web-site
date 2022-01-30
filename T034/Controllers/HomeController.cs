﻿using Microsoft.AspNetCore.Mvc;
using T034.Core.Api;
using Ninject;
using OAuth2;

namespace T034.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        [Inject]
        public ISettingService SettingService { get; set; }

        public ActionResult Index()
        {
            var item = SettingService.GetStartPage();
            if(item == null || item.Value == "")
                return View();

            return Redirect(item.Value);
        }
        public ActionResult Sites()
        {
            return View();
        }

        public ActionResult Auth()
        {
            return PartialView("AuthPartialView", UserInfo);
        }
    }
}
