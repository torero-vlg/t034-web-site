using System.Web.Mvc;
using T034.ViewModel;

namespace T034.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            var model = new LogonViewModel();
            return View(model);
        }

    }
}
