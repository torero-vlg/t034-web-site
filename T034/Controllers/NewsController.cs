using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Db.Entity;

namespace T034.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult Index()
        {
            var list = MvcApplication.Db.Select<News>();

            return View(list);
        }

        public ActionResult Single(int newsId)
        {
            var news = MvcApplication.Db.Get<News>(newsId);
            return View(news);
        }

    }
}
