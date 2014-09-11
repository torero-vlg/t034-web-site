using System.Web.Mvc;
using T034.Tools.Auth;

namespace T034.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Sites");
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
