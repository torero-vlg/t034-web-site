using System.Web.Mvc;

namespace T034.Controllers
{
    public class ErrorsController : Microsoft.AspNetCore.Mvc.Controller
    {
        public Microsoft.AspNetCore.Mvc.ActionResult Unauthorized()
        {
            if (!Request.IsAjaxRequest())
                return View();


            return Json(new { });
        }

        public Microsoft.AspNetCore.Mvc.ActionResult NotFound()
        {
            if (!Request.IsAjaxRequest())
                return View();


            return Json(new { });
        }

        public Microsoft.AspNetCore.Mvc.ActionResult InternalServerError()
        {
            if (!Request.IsAjaxRequest())
                return View();

            return Json(new { });
        }
    }
}
