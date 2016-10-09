using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Db.DataAccess;
using Ninject;
using NLog;
using OAuth2;
using OAuth2.Client;
using OAuth2.Models;
using T034.Repository;

namespace T034.Controllers
{
    public class BaseController : Controller
    {
        [Inject]
        public IBaseDb Db { get; set; }

        [Inject]
        public IRepository Repository { get; set; }

        protected readonly AuthorizationRoot AuthorizationRoot;

        private const string ProviderNameKey = "providerName";

        protected string ProviderName
        {
            get { return (string)Session[ProviderNameKey]; }
            set { Session[ProviderNameKey] = value; }
        }

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected UserInfo UserInfo;

        public BaseController(AuthorizationRoot authorizationRoot)
        {
            AuthorizationRoot = authorizationRoot;
        }

        protected IClient GetClient()
        {
            return AuthorizationRoot.Clients.FirstOrDefault(c => c.Name == ProviderName);
        }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (controllerName == "Base") return;

            var actionName = context.ActionDescriptor.ActionName;

            var user = "";
            Logger.Trace($"Controller: {controllerName}, Action: {actionName}, UserHost: {Request.UserHostAddress}, User:{user}, Request: {Request?.Url?.Query}, Request.QueryString: {Request?.QueryString}");

            if(controllerName.ToLower() != "account" && actionName.ToLower() != "auth")
                SetUserInfo();

            base.OnActionExecuting(context);
        }

        private void SetUserInfo()
        {
            try
            {
                var authCode = Request.Cookies["auth_code"];

                var nameValueCollection = new NameValueCollection();
                if (authCode != null)
                {
                    Logger.Trace($"Устанавливаем code: {authCode.Value}.");
                    nameValueCollection.Add("code", authCode.Value);
                }

                var str = nameValueCollection.AllKeys.Aggregate("",
                    (current, key) => current + $"{key}[{nameValueCollection[key]}]");
                Logger.Trace($"Получаем информацию о пользователе. nameValueCollection: {str}.");
                var client = GetClient();
                var userInfo = client?.GetUserInfo(nameValueCollection);
                if (userInfo != null)
                {
                    Logger.Trace($"Cервис авторизации: {client.Name}. Пользователь: {userInfo.Email}.");
                    UserInfo = userInfo;
                }
                else
                {
                    Logger.Trace("Не удалось получить пользователя.");
                    UserInfo = new UserInfo();
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = context.ActionDescriptor.ActionName;
            var controllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
            var user = "";
            Logger.Trace($"Controller: {controllerName}, Action: {actionName}, UserHost: {Request.UserHostAddress}, User:{user}, Request: {Request?.Url?.Query}");

            base.OnActionExecuted(context);
        }
    }
}