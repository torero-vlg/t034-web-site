using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Db.Entity;
using Db.Entity.Administration;
using T034.Tools.Attribute;
using T034.Tools.Auth;

namespace T034.Controllers
{
    public class NewsController :BaseController
    {
        public ActionResult Index()
        {
            var list = Db.Select<News>();

            return View(list);
        }

        public ActionResult Single(int newsId)
        {
            var news = Db.Get<News>(newsId);
            return View(news);
        }

        [HttpGet]
        [Role("Moderator")]
        public ActionResult Add()
        {
            return View();
        }

        [Role("Moderator")]
        public ActionResult Add(News news)
        {
            var user = YandexAuth.GetUser(Request);

            //найдём пользователя в БД
            var list = Db.Where<User>(u => u.Login == user.display_name);
            if (list.Any())
            {
                news.LogDate = DateTime.Now;
                news.User = new User {Id = list.FirstOrDefault().Id};

                var result = Db.Save(news);
                return RedirectToAction("Single", new {newsId = result});
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
