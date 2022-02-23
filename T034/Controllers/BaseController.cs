using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using T034.Core.DataAccess;
using Ninject;
using NLog;
using OAuth2;
using OAuth2.Client;
using OAuth2.Models;
using T034.Profiles;
using T034.Repository;
using System.Text;

namespace T034.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IBaseDb Db;

        protected readonly AuthorizationRoot AuthorizationRoot;

        private const string ProviderNameKey = "providerName";

        protected string ProviderName
        {
            get
            {
                HttpContext.Session.TryGetValue(ProviderNameKey, out byte[] providerName);
                return Encoding.UTF8.GetString(providerName);
            }
            set => HttpContext.Session.Set(ProviderNameKey, Encoding.UTF8.GetBytes(value));
        }

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected UserInfo UserInfo;

        public BaseController(AuthorizationRoot authorizationRoot,
            IBaseDb db)
        {
            AuthorizationRoot = authorizationRoot;
            Db = db;
        }

        protected IClient GetClient()
        {
            return AuthorizationRoot.Clients.FirstOrDefault(c => c.Name == ProviderName);
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            //TODO t-29 get controllerName
            //var controllerName = context.ActionDescriptor.D ControllerDescriptor.ControllerName;
            var controllerName = "";
            if (controllerName == "Base") return;

            var actionName = context.ActionDescriptor.DisplayName;

            var user = "";
            Logger.Trace($"Controller: {controllerName}, Action: {actionName}, UserHost: {HttpContext.Connection.RemoteIpAddress}, User:{user}, Request.QueryString: {Request?.QueryString}");

            if (controllerName.ToLower() != "account" && actionName.ToLower() != "auth")
                SetUserInfo();

            base.OnActionExecuting(context);
        }

        private void SetUserInfo()
        {
            try
            {
                if (Request.Cookies["auth"] != null)
                {
                    UserInfo = new UserInfo
                    {
                        Email = Request.Cookies["auth"]
                    };
                    return;
                }

                var nameValueCollection = new NameValueCollection();
                
                if (!string.IsNullOrEmpty(HttpContext.Request.Query["code"]))
                {
                    foreach(var key in Request.Query.Keys)
                        nameValueCollection.Add(key, Request.Query[key]);

                    Logger.Trace($"nameValueCollection заполняем из Request.QueryString[code].");
                }
                else
                {
                    var authCodeCookie = Request.Cookies["auth_code"];
                    if (authCodeCookie != null)
                    {
                        Logger.Trace($"Устанавливаем code: {authCodeCookie}.");
                        nameValueCollection.Add("code", authCodeCookie);
                    }
                }

                Logger.Trace($"nameValueCollection: {nameValueCollection}.");
                var client = GetClient();
                Logger.Trace($"Cервис авторизации: {client}. ProviderName:{ProviderName}.");

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

        public override void OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext context)
        {
            //TODO t-29 get controllerName actionName
            //var controllerName = context.ActionDescriptor.D ControllerDescriptor.ControllerName;

            //var actionName = context.ActionDescriptor.ActionName;
            var actionName = context.ActionDescriptor.DisplayName;
            //var controllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
            var controllerName = "";
            var user = "";
            Logger.Trace($"Controller: {controllerName}, Action: {actionName}, UserHost: {HttpContext.Connection.RemoteIpAddress}, User:{user}");

            base.OnActionExecuted(context);
        }

        /// <summary>
        /// Маппер
        /// </summary>
        protected IMapper Mapper => AutoMapperConfig.Mapper;
    }
}