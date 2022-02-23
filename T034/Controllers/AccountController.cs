using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using T034.Core.Entity.Administration;
using OAuth2;
using OAuth2.Models;
using T034.ViewModel;
using Microsoft.AspNetCore.Mvc;
using T034.Core.DataAccess;

namespace T034.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(AuthorizationRoot authorizationRoot, IBaseDb db) : base(authorizationRoot, db)
        {
        }

        /// <summary>
        /// Renders home page with login link.
        /// </summary>
        public ActionResult Logon(LogonViewModel model)
        {
            try
            {
                var clients = AuthorizationRoot.Clients.Select(client => new LoginInfoModel
                {
                    ProviderName = client.Name
                });
                model.Clients = clients;
                return View(model);
            }
            catch (Exception ex)
            {
                model.Clients = new List<LoginInfoModel>();
                return View(model);
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
                Logger.Trace(HttpContext.Request.Query["code"]);
                HttpContext.Response.Cookies.Append("auth_code",
                   HttpContext.Request.Query["code"],
                   new Microsoft.AspNetCore.Http.CookieOptions
                   {
                       Expires = DateTime.Now.AddDays(30)
                   });

                Logger.Trace($"Куки auth_code установлены: HttpContext.Request.Query['code']={HttpContext.Request.Query["code"]}, HttpContext.Response.Cookies={HttpContext.Response.Cookies}");

                Logger.Trace($"Получаем информацию о пользователе. Request.QueryString: {Request.QueryString}.");
                var client = GetClient();
                Logger.Trace($"Cервис авторизации: {client}");

                var nameValueCollection = new NameValueCollection();
                foreach (var key in Request.Query.Keys)
                    nameValueCollection.Add(key, Request.Query[key]);

                UserInfo = client?.GetUserInfo(nameValueCollection) ?? new UserInfo();
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

                HttpContext.Response.Cookies.Append("roles",
                    string.Join(",", user.UserRoles.Select(r => r.Code)),
                    new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)
                    });
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
                Logger.Trace(Request.Cookies["auth_code"]);
                nameValueCollection.Add("code", Request.Cookies["auth_code"]);
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
