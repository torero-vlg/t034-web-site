using System;
using System.IO;
using System.Web.Mvc;
using Db.Tools;
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
            var model = new UserModel{IsAutharization = false};
            try
            {
                
                if (Request.Cookies["user_token"] != null)
                {
                    var userCookie = Request.Cookies["user_token"];

                    var stream = HttpTools.PostStream("https://login.yandex.ru/info",
                                                        string.Format("oauth_token={0}",
                                                                    Server.HtmlEncode(userCookie.Value)));
                    model = SerializeTools.Deserialize<UserModel>(stream);
                    model.IsAutharization = true;
                }
            }
            catch (Exception ex)
            {
                MonitorLog.WriteLog(GetType() + " " + ex.InnerException + ex.Message, MonitorLog.typelog.Error, true);
                model.IsAutharization = false;
            }

            return PartialView("AuthPartialView", model);
        }
    }
}
