using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Db.Entity.Administration;
using OAuth2;
using OAuth2.Models;
using T034.ViewModel;

namespace T034.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(AuthorizationRoot authorizationRoot) : base(authorizationRoot)
        {
        }

        /// <summary>
        /// Renders home page with login link.
        /// </summary>
        public ActionResult Logon()
        {
            var clients = AuthorizationRoot.Clients.Select(client => new LoginInfoModel
            {
                ProviderName = client.Name
            });
            return View(new LogonViewModel { Clients = clients});
        }

        /// <summary>
        /// Redirect to login url of selected provider.
        /// </summary>        
        public RedirectResult Login(string providerName)
        {
            ProviderName = providerName;
            return new RedirectResult(GetClient().GetLoginLinkUri());
        }

        /// <summary>
        /// Renders information received from authentication service.
        /// </summary>
        public ActionResult Auth()
        {
            var userCookie = new HttpCookie("auth_code")
            {
                Value = Request.QueryString["code"],
                Expires = DateTime.Now.AddDays(30)
            };
            Response.Cookies.Set(userCookie);
            Logger.Trace($"Куки auth_code установлены: Request.QueryString[code]={Request.QueryString["code"]}, Response.Cookies[auth_code]={Response.Cookies["auth_code"].Value}");

            UserInfo = GetClient()?.GetUserInfo(Request.QueryString) ?? new UserInfo();

            var user = Db.SingleOrDefault<User>(u => u.Email == UserInfo.Email);

            var rolesCookie = new HttpCookie("roles") { Value = string.Join(",", user.UserRoles.Select(r => r.Code)), Expires = DateTime.Now.AddDays(30) };
            Response.Cookies.Set(rolesCookie);

            return View(UserInfo);
        }
    }
}
