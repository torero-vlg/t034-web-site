using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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


            //TODO первоначальная инициализация пользователя код скопирован из BaseController.OnActionExecuting
            //////////
            try
            {
                var authCode = Request.Cookies["auth_code"];
                Logger.Trace($"Request.Cookies[auth_code]: {Request.Cookies["auth_code"]}");

                var nameValueCollection = new NameValueCollection();
                if (authCode != null)
                {
                    Logger.Trace($"Request.QueryString[code]: {Request.QueryString["code"]}");
                    if (Request.QueryString["code"] == null)
                    {
                        Logger.Trace($"Устанавливаем code: {authCode.Value}.");
                        nameValueCollection.Add("code", authCode.Value);
                    }
                    else
                    {
                        nameValueCollection = Request.QueryString;
                    }
                }

                var str = nameValueCollection.AllKeys.Aggregate("", (current, key) => current + $"{key}[{nameValueCollection[key]}]");
                Logger.Trace($"Получаем информацию о пользователе. nameValueCollection: {str}.");
                UserInfo = GetClient()?.GetUserInfo(nameValueCollection) ?? new UserInfo();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }
            //////////

            return View(UserInfo);
        }
    }
}
