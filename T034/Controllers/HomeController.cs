using System.Linq;
using System.Web.Mvc;
using Db.Entity;
using T034.Tools.Auth;

namespace T034.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var item = Db.Where<Setting>(s => s.Code == "StartPage").FirstOrDefault();

            if(item == null)
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
