using System;
using System.Web.Mvc;
using Db.Entity;
using Db.Entity.Administration;

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

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Add(News news)
        {
            news.LogDate = DateTime.Now;
            news.User = new User { Id = 1 };

            var result = MvcApplication.Db.Save(news);
            return RedirectToAction("Single", new {newsId = result});
        }
    }
}
