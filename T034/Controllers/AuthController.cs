using System;
using System.Web;
using System.Web.Mvc;
using T034.Models;
using T034.Tools;

namespace T034.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            var client_id = "030edcedc0264dc188a18f4779642970";//код приложения
            var client_secret = "8f71a3459a104af9a9e05e52af8b03cd";//пароль приложения
            var code = Request.QueryString["code"];

            var stream = HttpTools.PostStream("https://oauth.yandex.ru/token", string.Format("grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}", code, client_id, client_secret));

            var model = SerializeTools.Deserialize<TokenModel>(stream);

            var userCookie = new HttpCookie("user_token")
                {
                    Value = model.access_token,
                    Expires = DateTime.Now.AddDays(30)
                };
            Response.Cookies.Set(userCookie);

            return RedirectToActionPermanent("Index", "Home");
        }

        public ActionResult Logout()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;

            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Value = "";
                Response.Cookies.Set(aCookie);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
