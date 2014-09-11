using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Db.Entity;
using Db.Entity.Administration;
using T034.Tools.Auth;

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
        [AuthorizeUser]
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Add(News news)
        {
            var user = YandexAuth.GetUser(Request);

            //найдём пользователя в БД
            var list = MvcApplication.Db.Where<User>(u => u.Login == user.display_name);
            if (list.Any())
            {
                news.LogDate = DateTime.Now;
                news.User = new User {Id = list.FirstOrDefault().Id};

                var result = MvcApplication.Db.Save(news);
                return RedirectToAction("Single", new {newsId = result});
            }
            return RedirectToAction("Index", "Home");
        }
    }

    public class AuthorizeUserAttribute : AuthorizeAttribute 
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies["yandex_token"] != null)
                return true;
            return false;
        }
    }
}
