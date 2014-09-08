using System;
using System.IO;
using System.Web.Mvc;
using T034.Models;
using T034.Tools;

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

            var model = new UserModel();
            try
            {
                if (Request.Cookies["user_token"] != null)
                {
                    var stream = HttpTools.PostStream("https://login.yandex.ru/info", string.Format("oauth_token={0}", Request.Cookies["user_token"].Value));
                    model = SerializeTools.Deserialize<UserModel>(stream);
                }
                model.IsAutharization = Request.Cookies["user_token"] != null;
            }
            catch (Exception ex)
            {
                model.IsAutharization = false;
            }

            return PartialView("AuthPartialView", model);
        }
    }
}
