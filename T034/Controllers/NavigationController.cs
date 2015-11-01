using System.Web.Mvc;

namespace T034.Controllers
{
    public class NavigationController : Controller
    {
        public ActionResult MainMenu()
        {
            return PartialView();
        }

        public ActionResult ManagementMenu()
        {
            return PartialView();
        }
    }
}
