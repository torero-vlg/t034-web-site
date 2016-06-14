using System.Web.Mvc;
using Db.Api;
using Ninject;
using T034.Tools.Auth;

namespace T034.Controllers
{
    public class HomeController : BaseController
    {
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
            var model = YandexAuth.GetUser(Request);

            return PartialView("AuthPartialView", model);
        }
    }
}
