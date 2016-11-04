using System;
using System.Collections.Generic;
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
        public ActionResult Logon(string message = null)
        {
            try
            {
                var clients = AuthorizationRoot.Clients.Select(client => new LoginInfoModel
                {
                    ProviderName = client.Name
                });
                return View(new LogonViewModel { Clients = clients, Message = message});
            }
            catch (Exception ex)
            {
                return View(new LogonViewModel { Clients = new List<LoginInfoModel>()});
            }
            
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
            try
            {
                Logger.Trace(Request.QueryString["code"]);
                var userCookie = new HttpCookie("auth_code")
                {
                    Value = Request.QueryString["code"],
                    Expires = DateTime.Now.AddDays(30)
                };
                Response.Cookies.Set(userCookie);
                Logger.Trace($"Куки auth_code установлены: Request.QueryString[code]={Request.QueryString["code"]}, Response.Cookies[auth_code]={Response.Cookies["auth_code"].Value}");

                Logger.Trace($"Получаем информацию о пользователе. Request.QueryString: {Request.QueryString}.");
                var client = GetClient();
                Logger.Trace($"Cервис авторизации: {client}");

                UserInfo = client?.GetUserInfo(Request.QueryString) ?? new UserInfo();
                Logger.Trace($"Пользователь: {UserInfo.Email}");

                //try
                //{
                //    Logger.Trace($"Делаем повторный запрос: {UserInfo.Email}");
                //    UserInfo = client?.GetUserInfo(Request.QueryString) ?? new UserInfo();
                //    Logger.Trace($"Пользователь2: {UserInfo.Email}");
                //}
                //catch (Exception ex)
                //{
                //    Logger.Fatal(ex);
                //}

                var user = Db.SingleOrDefault<User>(u => u.Email == UserInfo.Email);
                Logger.Trace($"Пользователь из БД: {user.Email}");

                var rolesCookie = new HttpCookie("roles") { Value = string.Join(",", user.UserRoles.Select(r => r.Code)), Expires = DateTime.Now.AddDays(30) };
                Response.Cookies.Set(rolesCookie);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return View(UserInfo);
        }

        /// <summary>
        /// Renders information received from authentication service.
        /// </summary>
        public ActionResult Auth2()
        {
            Logger.Trace(Request.Cookies["auth_code"]);
            var nameValueCollection = new NameValueCollection();

            if (Request.Cookies["auth_code"] != null)
            {
                Logger.Trace(Request.Cookies["auth_code"].Value);
                nameValueCollection.Add("code", Request.Cookies["auth_code"].Value);
            }

            Logger.Trace($"Получаем информацию о пользователе. Request.QueryString: {Request.QueryString}.");
            var client = GetClient();
            Logger.Trace($"Cервис авторизации: {client}");

            UserInfo = client?.GetUserInfo(nameValueCollection) ?? new UserInfo();
            Logger.Trace($"Пользователь: {UserInfo.Email}");

            return View("Auth", UserInfo);
        }
    }
}
