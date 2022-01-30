using System.Web.Mvc;
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

        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            var item = SettingService.GetStartPage();
            if(item == null || item.Value == "")
                return View();

            return Redirect(item.Value);
        }
        public Microsoft.AspNetCore.Mvc.ActionResult Sites()
        {
            return View();
        }

        public Microsoft.AspNetCore.Mvc.ActionResult Auth()
        {
            return PartialView("AuthPartialView", UserInfo);
        }
    }
}
