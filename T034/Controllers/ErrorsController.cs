using Microsoft.AspNetCore.Mvc;

namespace T034.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Unauthorized()
        {
            if (!IsAjax)
                return View();

            return Json(new { });
        }

        public ActionResult NotFound()
        {
            if (!IsAjax)
                return View();

            return Json(new { });
        }

        public ActionResult InternalServerError()
        {
            if (!IsAjax)
                return View();

            return Json(new { });
        }

        private bool IsAjax => Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
