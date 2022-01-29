using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using T034.Core.Entity;
using T034.Core.Services.Administration;
using Ninject;
using OAuth2;
using T034.Tools.Auth;
using T034.ViewModel;

namespace T034.Controllers
{
    public class AuthController : BaseController
    {
        [Inject]
        public IUserService UserService { get; set; }

        public AuthController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        public ActionResult LoginWithYandex(string code)
        {
            //            var userCookie = YandexAuth.GetAuthorizationCookie(Request);
            //  MonitorLog.WriteLog(string.Format("GetAuthorizationCookie({0})", Repository), MonitorLog.typelog.Info, true);
            var accessToken = YandexAuth.GetAuthorizationCookie(Response.Cookies, code, Db);
            //  MonitorLog.WriteLog(string.Format("accessToken = {0}", accessToken), MonitorLog.typelog.Info, true);

            FormsAuthentication.SetAuthCookie(accessToken, true);

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

        public ActionResult RedirectToYandex()
        {
            var clientId = Db.SingleOrDefault<Setting>(s => s.Code == "YandexClientId").Value;

            return Redirect(string.Format("https://oauth.yandex.ru/authorize?response_type=code&client_id={0}", clientId)); 
        }

        public ActionResult Login(LogonViewModel model)
        {
            var result = UserService.Authenticate(model.Email, model.Password);

            if (result.IsAuthenticated)
            {
                var rolesCookie = new HttpCookie("roles") { Value = string.Join(",", result.User.UserRoles.Select(r => r.Code)), Expires = DateTime.Now.AddDays(30) };
                var authCookie = new HttpCookie("auth") { Value = result.User.Email, Expires = DateTime.Now.AddDays(30) };
                Response.Cookies.Set(rolesCookie);
                Response.Cookies.Set(authCookie);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Logon", "Account",  new LogonViewModel { Email = model.Email, Message = result.Message});
            }
        }
    }
}
