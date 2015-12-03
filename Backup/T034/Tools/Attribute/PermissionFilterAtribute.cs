using System.Linq;
using System.Web.Mvc;
using Db.DataAccess;
using Db.Entity.Administration;
using Ninject;
using T034.Tools.Auth;

namespace T034.Tools.Attribute
{
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        [Inject]
        public IBaseDb Db { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var action = filterContext.ActionDescriptor.ActionName;
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var role = MvcApplication.ActionRoles.FirstOrDefault(p => p.Action == action.ToLower() && p.Controller == controller.ToLower());
            if (role == null) return;

            //получим email TODO не работает, так как Db = null
            //var user = YandexAuth.GetUser(filterContext.RequestContext.HttpContext.Request);
            //var userFromDb = Db.Where<User>(u => u.Email == user.default_email).FirstOrDefault();
            //if (userFromDb == null)
            //{
            //    filterContext.Result = new RedirectResult("~/Errors/Unauthorized");
            //    return;
            //}

            //if (userFromDb.UserRoles.Any(ur => ur.Code == role.Role))
            //    return;

            //if (filterContext.RequestContext.HttpContext.User.HasPermission(role.role)) return;
            var rolesCookie = filterContext.RequestContext.HttpContext.Request.Cookies["roles"];
            if (rolesCookie != null && rolesCookie.Value != null && rolesCookie.Value.Contains(role.Role))
                return;
            
            filterContext.Result = new RedirectResult("~/Errors/Unauthorized");

        }
    }

    public class Http403Result : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 403;
        }
    }
}