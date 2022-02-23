using Microsoft.AspNetCore.Mvc;
using T034.Core.Api;
using Ninject;
using OAuth2;

namespace T034.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISettingService _settingService;

        public HomeController(AuthorizationRoot authorizationRoot,
            ISettingService settingService) 
            : base(authorizationRoot)
        {
            _settingService = settingService;
        }

        public ActionResult Index()
        {
            var item = _settingService.GetStartPage();
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
