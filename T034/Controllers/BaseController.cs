using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using T034.Core.DataAccess;
using NLog;
using T034.Profiles;
using Microsoft.AspNetCore.Mvc.Controllers;

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
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerName;

            if (controllerName == "Base") return;

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
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerName;

            var user = "";
            Logger.Trace($"Controller: {controllerName}, Action: {actionName}, UserHost: {HttpContext.Connection.RemoteIpAddress}, User:{user}");

            base.OnActionExecuted(context);
        }

        /// <summary>
        /// Маппер
        /// </summary>
        protected IMapper Mapper => AutoMapperConfig.Mapper;
    }

    public class UserInfo
    {
        public string Email { get; set; }
    }
}