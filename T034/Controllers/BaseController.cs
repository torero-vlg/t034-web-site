﻿using System.Web.Mvc;
using System.Web.Routing;
using Db.DataAccess;
using Ninject;
using NLog;
using T034.Repository;

namespace T034.Controllers
{
    public class BaseController : Controller
    {
        [Inject]
        public IBaseDb Db { get; set; }

        [Inject]
        public IRepository Repository { get; set; }

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (controllerName == "Base") return;

            var actionName = context.ActionDescriptor.ActionName;

            var user = "";
            Logger.Debug("Controller: {0}, Action: {1}, UserHost: {2}, User:{3}, Request: {4}", controllerName, actionName, Request.UserHostAddress, user, Request?.Url?.Query);

            base.OnActionExecuting(context);
        }

        protected override void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = context.ActionDescriptor.ActionName;
            var controllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
            var user = "";
            Logger.Debug("Controller: {0}, Action: {1}, UserHost: {2}, User:{3}, Request: {4}", controllerName, actionName, Request.UserHostAddress, user, Request?.Url?.Query);

            base.OnActionExecuted(context);
        }
    }
}