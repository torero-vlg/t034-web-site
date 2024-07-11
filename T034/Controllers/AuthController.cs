using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Entity;
using T034.Core.Services.Administration;
using T034.Tools.Auth;
using T034.ViewModel;
using T034.Core.DataAccess;
using AutoMapper;

namespace T034.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService, IBaseDb db, IMapper mapper) 
            : base(db, mapper)
        {
            _userService = userService;
        }

        public ActionResult LoginWithYandex(string code)
        {
            //            var userCookie = YandexAuth.GetAuthorizationCookie(Request);
            //  MonitorLog.WriteLog(string.Format("GetAuthorizationCookie({0})", Repository), MonitorLog.typelog.Info, true);
            var accessToken = YandexAuth.GetAuthorizationCookie(Response.Cookies, code, Db);
            //  MonitorLog.WriteLog(string.Format("accessToken = {0}", accessToken), MonitorLog.typelog.Info, true);

           //TODO t-29
        //    FormsAuthentication.SetAuthCookie(accessToken, true);

            return RedirectToActionPermanent("Index", "Home");
        }

        public ActionResult Logout()
        {
            foreach (var key in Request.Cookies.Keys)
                HttpContext.Response.Cookies.Delete(key);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult RedirectToYandex()
        {
            var clientId = Db.SingleOrDefault<Setting>(s => s.Code == "YandexClientId").Value;

            return Redirect(string.Format("https://oauth.yandex.ru/authorize?response_type=code&client_id={0}", clientId)); 
        }

        public ActionResult Login(LogonViewModel model)
        {
            var result = _userService.Authenticate(model.Email, model.Password);

            if (result.IsAuthenticated)
            {
                HttpContext.Response.Cookies.Append("auth",
                    result.User.Email,
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)
                    });

                HttpContext.Response.Cookies.Append("roles",
                    string.Join(",", result.User.UserRoles.Select(r => r.Code)),
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)
                    });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Logon", "Account",  new LogonViewModel { Email = model.Email, Message = result.Message});
            }
        }
    }
}
