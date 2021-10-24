using System.Web.Mvc;

namespace T034.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Unauthorized()
        {
            if (!Request.IsAjaxRequest())
                return View();


            return Json(new { });
        }

        public ActionResult NotFound()
        {
            if (!Request.IsAjaxRequest())
                return View();


            return Json(new { });
        }

        public ActionResult InternalServerError()
        {
            if (!Request.IsAjaxRequest())
                return View();

            return Json(new { });
        }
    }
}
