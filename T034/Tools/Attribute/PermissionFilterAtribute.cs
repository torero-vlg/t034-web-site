using System.Linq;

namespace T034.Tools.Attribute
{
    public class PermissionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
        {
            //TODO t-29 get controllerName action
            //var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var controller = "";

            var action = filterContext.ActionDescriptor.DisplayName;

            //var role = MvcApplication.ActionRoles.FirstOrDefault(p => p.Action == action.ToLower() && p.Controller == controller.ToLower());
            var role = Program.ActionRoles.FirstOrDefault(p => p.Action == action.ToLower() && p.Controller == controller.ToLower());
            if (role == null) return;

            var rolesCookie = filterContext.HttpContext.Request.Cookies["roles"];
            if (rolesCookie != null && rolesCookie != null && rolesCookie.Contains(role.Role))
                return;
            
            filterContext.Result = new Microsoft.AspNetCore.Mvc.RedirectResult("~/Errors/Unauthorized");
        }
    }
}