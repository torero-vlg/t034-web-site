using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using T034.Core.Api;
using T034.Core.DataAccess;

namespace T034.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISettingService _settingService;

        public HomeController(ISettingService settingService,
            IBaseDb db, IMapper mapper) 
            : base(db, mapper)
        {
            _settingService = settingService;
        }

        public ActionResult Index()
        {
            var item = _settingService.GetStartPage();
            if(item == null || item.Value == "")
                return View();

            return Redirect(item.Value);
        }
        public ActionResult Sites()
        {
            return View();
        }

        public ActionResult Auth()
        {
            return PartialView("AuthPartialView", UserInfo);
        }

        public ActionResult AwaitingPayment()
        {
            return View("_AwaitingPayment");
        }

        public ActionResult Developing()
        {
            return View("_Developing");
        }

        public ActionResult Moved()
        {
            return View("_Moved");
        }
    }
}
