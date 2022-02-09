using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T034.Core.DataAccess;
using Ninject;

namespace T034.Tools.Attribute
{
    public class PermissionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        [Inject]
        public IBaseDb Db { get; set; }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
        {
            var action = filterContext.ActionDescriptor.ActionName;
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var role = MvcApplication.ActionRoles.FirstOrDefault(p => p.Action == action.ToLower() && p.Controller == controller.ToLower());
            if (role == null) return;

            var rolesCookie = filterContext.RequestContext.HttpContext.Request.Cookies["roles"];
            if (rolesCookie != null && rolesCookie.Value != null && rolesCookie.Value.Contains(role.Role))
                return;
            
            filterContext.Result = new Microsoft.AspNetCore.Mvc.RedirectResult("~/Errors/Unauthorized");
        }
    }
}