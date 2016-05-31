using System.Web.Mvc;

namespace T034.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Unauthorized()
        {
            return View("ServerError", (object)"Недостаточно прав");
        }
    }
}
