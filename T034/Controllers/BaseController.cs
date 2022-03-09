using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using T034.Core.DataAccess;
using NLog;
using OAuth2;
using OAuth2.Models;
using T034.Profiles;
using System.Text;

namespace T034.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IBaseDb Db;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected UserInfo UserInfo;

        public BaseController(IBaseDb db)
        {
            Db = db;
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