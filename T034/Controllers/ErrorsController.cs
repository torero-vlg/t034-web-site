using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
